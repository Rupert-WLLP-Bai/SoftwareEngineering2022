using SimpleOJ.Common;
using SimpleOJ.Model;

namespace SimpleOJ.Controllers {
    public interface ILoginController {
        public record LoginUserInfo(User User, string Token);

        public Result<LoginUserInfo> Login(string id, string password);
    }
}
