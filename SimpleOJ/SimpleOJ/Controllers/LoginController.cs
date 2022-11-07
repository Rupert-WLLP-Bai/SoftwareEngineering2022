using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController {

        private readonly IUserService _userService;

        public LoginController() {
            _userService = new UserService();
        }

        [HttpPost("Login")]
        public Result Login(string? id,string? password) {
            var user = _userService.GetByUserId(id);
            // 检查用户是否存在
            if (user.Id == null) {
                // 不存在则返回用户名不存在
                // TODO 完成状态码的定义
                return new Result(ResultCode.Failure, null);
            }
            // 存在，则验证密码是否正确
            // TODO 完成密码加密和密码验证
            return new Result();
        }
    }
}
