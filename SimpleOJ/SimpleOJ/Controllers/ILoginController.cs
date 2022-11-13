using SimpleOJ.Common;
using SimpleOJ.Model;

namespace SimpleOJ.Controllers {
    public interface ILoginController {
        /// <summary>
        /// 登录返回的用户信息
        /// </summary>
        public class UserInfo {
            public User User { get; set; }
            public string Token { get; set; }
            public UserInfo(User user, string token) {
                User = user;
                Token = token;
            }
        }
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="password">用户密码</param>
        /// <returns>登陆成功: 返回用户信息和token</returns>
        /// <returns>登录失败: 返回错误信息</returns>
        Result Login(string? id, string? password);
    }
}
