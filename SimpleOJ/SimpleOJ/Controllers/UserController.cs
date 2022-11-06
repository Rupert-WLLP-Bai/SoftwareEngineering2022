using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJ.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    private readonly IUserService _userService;

    public UserController() {
        _userService = new UserService();
    }
    
    /// <summary>
    /// 条件查询
    /// </summary>
    /// <param name="user">查询条件</param>
    /// <returns>符合条件的用户列表</returns>
    [HttpPost("GetList")]
    public Result GetList([FromBody] User user) {
        return new Result(ResultCode.Success, _userService.GetList(user));
    } 

    /// <summary>
    /// 按角色查询
    /// </summary>
    /// <param name="role">角色枚举</param>
    /// <returns>符合对应角色的用户列表</returns>
    [HttpPost("GetListByRole")]
    public Result GetListByRole(IUserService.Role role) {
        return new Result(ResultCode.Success, _userService.GetByRole(role));
    }
}
