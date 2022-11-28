using SimpleOJ.Common;
using SimpleOJ.Model;

namespace SimpleOJ.Controllers {
    public interface ILoginController {
        public record LoginParam(string? Id, string? Password);
        public record LoginUserInfo(User User, string Token);
        public record OutLoginUserInfo(User User, string Token, DateTime OutTime);
        public Result<LoginUserInfo> Login(LoginParam loginParam);
        public Result<OutLoginUserInfo> OutLogin();
    }
}
