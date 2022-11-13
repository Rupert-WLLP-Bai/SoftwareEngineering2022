using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Redis;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthTest {
        [TestMethod]
        [HttpGet("test")]
        [Authorize(Roles = "Admin")]
        public string Auth() {
            return "authorization success!";
        }
    }
}
