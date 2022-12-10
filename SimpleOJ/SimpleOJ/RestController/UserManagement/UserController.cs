using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJ.RestController.UserManagement {
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase {
        private readonly IUserService _userService;
        public UserController(IUserService userService) {
            _userService = userService;
        }

        [HttpGet]
        public Result<IEnumerable<User>> GetAll(int? current, int? pageSize) {
            var users = _userService.GetUsers(current,pageSize);
            return new Result<IEnumerable<User>>(true, ResultCode.Success, users);
        }

        [HttpGet("{id}")]
        public Result<IEnumerable<User>> GetUser(string id) {
            return new Result<IEnumerable<User>>(null);
        }

        [HttpDelete("{id}")]
        public Result<User> DeleteUser(string id) {
            return new Result<User>(null);
        }

        [HttpPut]
        public Result<User> PutUser(User user) {
            return new Result<User>(null);
        }
        
        [HttpPatch]
        public Result<User> PatchUser(User? user) {
            return new Result<User>(null);
        }
    }
}
