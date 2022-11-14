using log4net;
using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;
using SimpleOJ.Util;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class OldOldLoginController : IOldLoginController {

        private readonly IUserService _userService;

        private readonly IJwtTokenService _jwtTokenService;

        private static readonly ILog Log = LogManager.GetLogger(typeof(OldOldLoginController));

        public OldOldLoginController() {
            _userService = new UserService();
            _jwtTokenService = new JwtTokenService();
        }

        [HttpPost("Login")]
        public OldResult OldLogin(string? id, string? password) {
            var user = _userService.GetByUserId(id);
            // 检查用户是否存在
            if (user == null) {
                // 不存在则返回用户名不存在
                return new OldResult(OldResultCode.LoginUsernameNotExist, null);
            }
            // 存在，则验证密码是否正确
            var pwd = EncryptPassword.Encrypt(password!, user.Salt!);
            if (!pwd.Equals(user.Password)) {
                // 密码错误
                return new OldResult(OldResultCode.LoginPasswordError, null);
            }
            // 密码正确，在redis中查用户id
            if (!_jwtTokenService.ContainsKey(user.Id!)) {
                // 如果不存在，生成token
                var userRole = (User.UserRole)Enum.Parse(typeof(User.UserRole), user.Role!.ToString()!);
                var token = _jwtTokenService.GenerateToken(new UserToken(user.Id!, userRole), DateTime.Now);
                Log.Debug($"UserId:{user.Id} 不存在Token, 生成的Token:{token}");
                // 返回登陆成功，user数据和token
                return new OldResult(OldResultCode.LoginSuccess, new IOldLoginController.OldUserInfo(user, token));
            } else {
                // 如果存在token
                var tokenTime = JwtTokenService.client.GetValue(user.Id).TrimStart('\"').TrimEnd('\"');
                Log.Debug($"UserId:{user.Id} 存在Token, 已存在的Token的创建时间:{tokenTime}");
                //  更新Token
                var updatedToken = _jwtTokenService.UpdateToken(new UserToken(user.Id!,
                    (User.UserRole)Enum.Parse(typeof(User.UserRole), user.Role.ToString()!)));
                Log.Debug($"新生成的Token: {updatedToken}");
                return new OldResult(OldResultCode.LoginSuccess, new IOldLoginController.OldUserInfo(user, updatedToken));
            }
        }
        
        
    }
}
