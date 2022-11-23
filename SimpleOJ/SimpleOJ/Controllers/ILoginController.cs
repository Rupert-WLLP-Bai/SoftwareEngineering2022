using SimpleOJ.Common;
using SimpleOJ.Model;

namespace SimpleOJ.Controllers {
    public interface ILoginController {
        public record LoginParam(string? Id, string? Password);
        public record LoginUserInfo(User User, string Token);
        public Result<LoginUserInfo> Login(LoginParam loginParam);
    }
}
