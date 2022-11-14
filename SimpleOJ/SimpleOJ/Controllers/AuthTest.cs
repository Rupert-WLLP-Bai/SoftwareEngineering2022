using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthTest {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AuthTest));
        [TestMethod]
        [HttpGet("test")]
        [Authorize(Roles = "Admin")]
        public string Auth() {
            Log.Info("通过Authorize验证");
            return "authorization success!";
        }
    }
}
