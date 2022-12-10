using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;
using SimpleOJ.Util;

namespace SimpleOJ.RestController.UserManagement
{
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public Result<IEnumerable<User>> GetAll(int? current, int? pageSize)
    {
        var users = _userService.GetUsers(current, pageSize);
        return new Result<IEnumerable<User>>(true, ResultCode.Success, users);
    }

    [HttpGet("{id}")]
    public Result<IEnumerable<User>> GetUser(string id)
    {
        return new Result<IEnumerable<User>>(null);
    }

    [HttpDelete("{id}")]
    public Result<User> DeleteUser(string id)
    {
        return new Result<User>(null);
    }

    [HttpPut]
    public Result<User> PutUser(User user)
    {
        return new Result<User>(null);
    }

    [HttpPatch]
    public Result<User> PatchUser(User? user)
    {
        return new Result<User>(null);
    }

    [HttpPost]
    public Result<User> PostUser(User? user) {
        // 用户为null返回失败
        if (user == null) {
            return new Result<User>(false, ResultCode.Failure, null);
        }
        // 随机盐值
        var salt = Guid.NewGuid().ToString();

        // 如果用户密码为空，则设置密码为123456
        if (string.IsNullOrEmpty(user.Password)) {
            user.Password = EncryptPassword.Encrypt("123456", salt);
            user.Salt = salt;
        }
        // 如果用户密码不为空，则加密密码
        else {
            user.Password = EncryptPassword.Encrypt(user.Password, salt);
            user.Salt = salt;
        }

        user.CreateTime = DateTime.Now;
        user.UpdateTime = DateTime.Now;
        // 添加用户
        var result = _userService.AddUser(user);
        return new Result<User>(true, ResultCode.Success, result);
    }
}
}
