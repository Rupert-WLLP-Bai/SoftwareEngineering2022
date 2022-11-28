using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;
using SimpleOJ.Util;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase, ILoginController {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUserLoginService _userLoginService;
        private readonly ILog _log = LogManager.GetLogger(typeof(LoginController));

        public LoginController(IUserService userService, IJwtTokenService jwtTokenService,
            IUserLoginService userLoginService) {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _userLoginService = userLoginService;
        }

        /// <summary>
        /// 登出，注销
        /// </summary>
        /// <returns></returns>
        [HttpPost("OutLogin")]
        [Authorize]
        public Result<ILoginController.OutLoginUserInfo> OutLogin() {
            // 获取用户的Token
            // 从请求头中获取token
            foreach (var (key, value) in Request.Headers) {
                _log.Debug($"{key} = {value}");
            }
            var authorization = Request.Headers["Authorization"].ToString();
            var token = string.Empty;
            if (authorization.Length > 7) {
                token = authorization[7..];
            } else {
                _log.Warn($"[OutLogin] [Bad Token] Request.Headers[\"Authorization\"] = {authorization}");
            }
            // 从token中获取用户信息
            _log.Debug($"token = {token}");
            var userId = string.Empty;
            try {
                userId = _jwtTokenService.ParseUserId(token);
            }
            catch {
                _log.Error($"token解析失败, token = {token}");
                return new Result<ILoginController.OutLoginUserInfo>(false, ResultCode.Failure, null);
            }
            // [DEBUG]输出
            _log.Debug($"[注销] 当前用户id为{userId}, token为{token}");
            var user = _userService.GetByUserId(userId);
            if (user==null) {
                return new Result<ILoginController.OutLoginUserInfo>(false, ResultCode.Failure, null);
            }
            // redis存在用户id
            if (_jwtTokenService.ContainsKey(userId)) {
                _log.Info($"用户{userId}申请注销, Redis中存在该用户");
                _jwtTokenService.DeleteToken(userId);
                return new Result<ILoginController.OutLoginUserInfo>(true, ResultCode.Success, new ILoginController.OutLoginUserInfo(user,token,DateTime.Now));
            }
            // redis不存在，用户已注销
            _log.Warn($"用户{userId}之前已经注销过, Redis中不存在该用户");
            return new Result<ILoginController.OutLoginUserInfo>(true, ResultCode.Success, new ILoginController.OutLoginUserInfo(user,token,DateTime.Now));
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginParam">登录参数</param>
        /// <returns>登录状态以及Token</returns>
        [HttpPost("Account")]
        public Result<ILoginController.LoginUserInfo> Login(ILoginController.LoginParam loginParam) {
            var id = loginParam.Id!;
            var password = loginParam.Password!;

            string resultToken;

            var iPv4 = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4();
            var remotePort = Request.HttpContext.Connection.RemotePort;
            _log.Info(
                $"调用{typeof(LoginController)},参数为: id = {id}, password = {password} \nIpv4 = {iPv4}, RemotePort = {remotePort.ToString()}");

            // 查询用户登录表
            var userLogin = _userLoginService.GetByUserId(userId: id);
            // 未登录过
            if (userLogin == null) {
                _log.Info($"用户{id}未登录过, 将创建新的登录记录");
                var newUserLogin = new UserLogin
                {
                    UserId = id,
                    LoginTime = DateTime.Now,
                    Ip = iPv4?.ToString()
                };
                var insertLogin = _userLoginService.InsertLogin(newUserLogin);
                if (insertLogin == false) {
                    // 插入失败
                    _log.Error($"数据库插入用户登录记录失败, 用户id = {id}");
                    throw new Exception("数据库插入用户登录记录失败");
                }
            } else {
                // 已登录过 更新登录时间
                _log.Info($"用户{id}已登录过, 上次登录时间为{userLogin.LoginTime.ToString()}");
                userLogin.LoginTime = DateTime.Now;
                userLogin.Ip = iPv4?.ToString();
                _userLoginService.UpdateLogin(userLogin);
                _log.Info($"更新用户{id}的登录时间为{userLogin.LoginTime.ToString()}");
            }

            var user = _userService.GetByUserId(id);
            // 检查用户是否存在
            if (user == null) {
                return new Result<ILoginController.LoginUserInfo>(false, ResultCode.LoginAccountNotExist, null);
            }

            // 验证密码是否正确
            var pwd = EncryptPassword.Encrypt(password, user.Salt!);
            if (!pwd.Equals(user.Password)) {
                return new Result<ILoginController.LoginUserInfo>(false, ResultCode.LoginPasswordIncorrect, null);
            }

            // 在Redis中查找用户id
            if (!_jwtTokenService.ContainsKey(user.Id!)) {
                // 不存在生成token
                var userRole = (User.UserRole)Enum.Parse(typeof(User.UserRole), user.Role!.ToString()!);
                var newToken = _jwtTokenService.GenerateToken(new UserToken(user.Id!, userRole), DateTime.Now);
                _log.Debug($"UserId:{user.Id} 不存在Token, 生成的Token:{newToken}");
                resultToken = newToken;
            } else {
                // 存在则更新token
                // 如果存在token
                var tokenTime = JwtTokenService.Client.GetValue(user.Id).TrimStart('\"').TrimEnd('\"');
                _log.Debug($"UserId:{user.Id} 存在Token, 已存在的Token的创建时间:{tokenTime}");
                //  更新Token
                var updatedToken = _jwtTokenService.UpdateToken(
                    new UserToken(user.Id!, (User.UserRole)Enum.Parse(typeof(User.UserRole), user.Role.ToString()!)));
                _log.Debug($"新生成的Token: {updatedToken}");
                resultToken = updatedToken;
            }

            // 返回登录信息
            _log.Info($"当前登录用户:{user}");
            return new Result<ILoginController.LoginUserInfo>(true,
                ResultCode.LoginSuccess,
                new ILoginController.LoginUserInfo(user, resultToken));
        }
    }
}
