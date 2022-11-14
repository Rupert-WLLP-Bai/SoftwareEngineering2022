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
        private readonly ILog _log;
        public LoginController() {
            _userService = new UserService();
            _jwtTokenService = new JwtTokenService();
            _log = LogManager.GetLogger(typeof(LoginController));
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="id">学工号</param>
        /// <param name="password">密码</param>
        /// <returns>登录状态以及Token</returns>
        [HttpPost("Login")]
        public Result<ILoginController.LoginUserInfo> Login(string id, string password) {
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

            string resultToken;
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
            _log.Debug($"当前登录用户:{user}");
            return new Result<ILoginController.LoginUserInfo>(true,
                ResultCode.LoginSuccess,
                new ILoginController.LoginUserInfo(user, resultToken));

        }
    }
}
