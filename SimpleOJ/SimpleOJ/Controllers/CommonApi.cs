using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;

namespace SimpleOJ.Controllers {

    [ApiController]
    [Route("api")]
    public class CommonApi : ControllerBase {
        private readonly ILog _log = LogManager.GetLogger(typeof(CommonApi));
        public record CurrentUserInfo(User User, string Token, string? IPv4);

        // TODO 完成该函数
        [HttpGet("CurrentUser")]
        public Result<CurrentUserInfo> GetCurrentUserInfo() {
            // TODO 调用ParseUserId
            // TODO 查询数据库
            // TODO updateToken
            // TODO 返回用户Info和Token
            var IPv4 = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            CurrentUserInfo userInfo = new CurrentUserInfo(new User(), "", IPv4);
            return new Result<CurrentUserInfo>(true, ResultCode.Success, null);
        }
    }
}
