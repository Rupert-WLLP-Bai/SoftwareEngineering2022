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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILog _log;

        public LoginController(IHttpContextAccessor httpContextAccessor,IUserService userService,IJwtTokenService jwtTokenService) {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _log = LogManager.GetLogger(typeof(LoginController));
            _httpContextAccessor = httpContextAccessor;
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

            // var ip = _httpContextAccessor.HttpContext?.Request.Headers["Origin"].FirstOrDefault();
            // _log.Info($"HTTP请求IP = {ip}");
            // if (ip == null) {
            //     _log.Warn("ip为null");
            // }

            var iPv4 = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4();
            var remotePort = Request.HttpContext.Connection.RemotePort;
            // var iPv6 = Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv6();
            // _log.Debug($"IPv4 = {iPv4}");
            // _log.Debug($"IPv6 = {iPv6}");
            // _log.Debug($"RemotePort = {remotePort}");
            _log.Info(
                $"调用{typeof(LoginController)},参数为: id = {id}, password = {password} \nIpv4 = {iPv4}, RemotePort = {remotePort.ToString()}");

            // if (_httpContextAccessor.HttpContext?.Request.Headers != null) {
            //     foreach (var (key, value) in _httpContextAccessor.HttpContext?.Request.Headers)
            //         _log.Info($"key = {key}, value = {value.ToString()}");
            // }

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
                var tokenTime = JwtTokenService.client.GetValue(user.Id).TrimStart('\"').TrimEnd('\"');
                _log.Debug($"UserId:{user.Id} 存在Token, 已存在的Token的创建时间:{tokenTime}");
                //  更新Token
                var updatedToken = _jwtTokenService.UpdateToken(new UserToken(user.Id!,
                    (User.UserRole)Enum.Parse(typeof(User.UserRole), user.Role.ToString()!)));
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
