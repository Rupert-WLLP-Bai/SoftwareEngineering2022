using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Service;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase {
        private readonly IJwtTokenService _jwtTokenService;

        public TestController(IJwtTokenService jwtTokenService) {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("GenToken")]
        public Result<string> GenToken([FromBody] UserToken userInfo) {
            return new Result<string>(true, ResultCode.Success, _jwtTokenService.GenerateToken(userInfo, DateTime.Now));
        }


        [HttpPost("DecodeToken")]
        public Result<object> DecodeToken([FromBody] string token) {
            return new Result<object>(true, ResultCode.Success, _jwtTokenService.DecodeJwtToken(token, null));
        }

        [HttpPost("UpdateToken")]
        public Result<string> Update(UserToken userInfo) {
            return new Result<string>(true, ResultCode.Success, _jwtTokenService.UpdateToken(userInfo));
        }

        [HttpPost("UpdateToken2")]
        public Result<string> UpdateToken(string token) {
            return new Result<string>(true, ResultCode.Success, _jwtTokenService.UpdateToken(token));
        }

        [HttpPost("VerifyToken")]
        public Result<JwtStatus> VerifyToken(string token) {
            return new Result<JwtStatus>(true, ResultCode.Success, _jwtTokenService.VerifyToken(token, null));
        }



    }
}
