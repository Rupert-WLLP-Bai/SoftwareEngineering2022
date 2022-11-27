using log4net;
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
