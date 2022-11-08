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
        public Result GenToken([FromBody] UserToken userInfo) {
            return new Result(ResultCode.Success,_jwtTokenService.GenerateToken(userInfo,DateTime.Now));
        }
        
        [HttpPost("DecodeToken")]
        public Result DecodeToken([FromBody] string token) {
            return new Result(ResultCode.Success,_jwtTokenService.DeocdeJwtToken(token,null));
        }
        
        [HttpPost("UpdateToken")]
        public Result Update(UserToken userInfo) {
            return new Result(ResultCode.Success,_jwtTokenService.UpdateToken(userInfo));
        }
        
        [HttpPost("UpdateToken2")]
        public Result UpdateToken(string token) {
            return new Result(ResultCode.Success,_jwtTokenService.UpdateToken(token));
        }
        
        [HttpPost("VerifyToken")]
        public Result VerifyToken(string token) {
            return new Result(ResultCode.Success,_jwtTokenService.VerifyToken(token,null));
        }

        
        
    }
}