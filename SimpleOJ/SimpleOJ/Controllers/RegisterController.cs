using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;
using SimpleOJ.Util;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController {
        private readonly IUserService _userService;
        public RegisterController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("Register")]
        public Result<User> Register(string id, string password, string name, string email, string phone) {
            // 无权限限制
            // 查询数据库中是否存在id
            var user = _userService.GetByUserId(id);
            if (user != null) {
                // 存在返回null
                return new Result<User>(false, ResultCode.RegisterAccountExist, null);
            }

            // TODO 验证密码的合法性
            // TODO 验证手机邮箱的合法性
            // 添加用户
            var salt = SaltGenerator.GenerateSalt();
            var newUser = new User()
            {
                Id = id,
                Name = name,
                Email = email,
                Phone = phone,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Role = (int?)User.UserRole.Student,
                Salt = salt,
                Password = EncryptPassword.Encrypt(password, salt),
                Status = (int?)User.UserStatus.Activated
            };
            // 调用service层
            if (_userService.AddUser(newUser) == null) {
                return new Result<User>(false, ResultCode.RegisterAddUserFailed, null);
            }

            return new Result<User>(true, ResultCode.RegisterSuccess, newUser);
        }
    }
}
