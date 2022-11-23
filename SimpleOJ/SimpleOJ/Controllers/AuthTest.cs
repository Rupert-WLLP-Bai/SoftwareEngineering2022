using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthTest {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthTest));
        private readonly IJwtTokenService _jwtTokenService;
        public AuthTest() {
            _jwtTokenService = new JwtTokenService();
        }
        public record AuthTestResult(string Message, string Token);
        [TestMethod]
        [HttpGet("test")]
        [Authorize(Roles = "Admin")]
        public Result<AuthTestResult> Auth() {
            Log.Info("通过Authorize验证");
            var updateToken = _jwtTokenService.UpdateToken(new UserToken("admin",User.UserRole.Admin));
            return new Result<AuthTestResult>(true, ResultCode.Success, 
                new AuthTestResult("authorization success!", updateToken));
        }
    }
}
