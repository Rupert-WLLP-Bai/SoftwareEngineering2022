using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;
using SimpleOJ.Util;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController {

        private readonly IUserService _userService;

        public LoginController() {
            _userService = new UserService();
        }

        [HttpPost("Login")]
        public Result Login(string? id, string? password) {
            var user = _userService.GetByUserId(id);
            // 检查用户是否存在
            if (user == null) {
                // 不存在则返回用户名不存在
                // TODO 完成状态码的定义
                return new Result(ResultCode.LoginUsernameNotExist, null);
            }
            // 存在，则验证密码是否正确
            var pwd = EncryptPassword.Encrypt(password!, user.Salt!);
            if (!pwd.Equals(user.Password)) {
                // 密码错误
                return new Result(ResultCode.LoginPasswordError, null);
            }
            // 密码正确，返回用户信息
            return new Result(ResultCode.Success, user);
        }
    }
}
