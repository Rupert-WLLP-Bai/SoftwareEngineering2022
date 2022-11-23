using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;

namespace SimpleOJ.Controllers {
    
    
    [ApiController]
    [Route("api")]
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Student")]
    [Authorize(Roles = "Assistant")]
    public class CommonApi {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public record CurrentUserInfo(User User, string Token);

        public CommonApi(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
        }
        
        // TODO 完成该函数
        [HttpGet("CurrentUser")]
        public Result<CurrentUserInfo> GetCurrentUserInfo() {
            // TODO 调用ParseUserId
            // TODO 查询数据库
            // TODO updateToken
            // TODO 返回用户Info和Token
            return new Result<CurrentUserInfo>(true, ResultCode.Success, null);
        }
    }
}
