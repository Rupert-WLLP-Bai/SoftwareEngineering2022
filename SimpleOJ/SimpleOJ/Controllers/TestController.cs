using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Service;

namespace SimpleOJ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        public TestController()
        {
            _jwtTokenService = new JwtTokenService();
        }

        [HttpPost("GenToken")]
        public OldResult GenToken([FromBody] UserToken userInfo) {
            return new OldResult(OldResultCode.Success,_jwtTokenService.GenerateToken(userInfo,DateTime.Now));
        }
        
        [HttpPost("DecodeToken")]
        public OldResult DecodeToken([FromBody] string token) {
            return new OldResult(OldResultCode.Success,_jwtTokenService.DecodeJwtToken(token,null));
        }
        
        [HttpPost("UpdateToken")]
        public OldResult Update(UserToken userInfo) {
            return new OldResult(OldResultCode.Success,_jwtTokenService.UpdateToken(userInfo));
        }
        
        [HttpPost("UpdateToken2")]
        public OldResult UpdateToken(string token) {
            return new OldResult(OldResultCode.Success,_jwtTokenService.UpdateToken(token));
        }
        
        [HttpPost("VerifyToken")]
        public OldResult VerifyToken(string token) {
            return new OldResult(OldResultCode.Success,_jwtTokenService.VerifyToken(token,null));
        }

        
        
    }
}