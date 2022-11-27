using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="user">查询条件</param>
        /// <returns>符合条件的用户列表</returns>
        [HttpPost("GetList")]
        public Result<IEnumerable<User>> GetList([FromBody] User user) {
            return new Result<IEnumerable<User>>(true, ResultCode.Success, _userService.GetList(user));
        }

        /// <summary>
        /// 按角色查询
        /// </summary>
        /// <param name="userRole">角色枚举</param>
        /// <returns>符合对应角色的用户列表</returns>
        [HttpGet("GetListByRole")]
        public Result<IEnumerable<User>> GetListByRole(User.UserRole userRole) {
            return new Result<IEnumerable<User>>(true, ResultCode.Success, _userService.GetByRole(userRole));
        }

        // TODO 添加用户接口
        // TODO 其他接口也要加入权限的限制
    }
}
