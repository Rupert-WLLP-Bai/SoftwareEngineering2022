using SimpleOJ.Common;
using SimpleOJ.Model;

namespace SimpleOJ.Controllers {
    public interface IOldLoginController {
        /// <summary>
        /// 登录返回的用户信息
        /// </summary>
        public class OldUserInfo {
            public User User { get; set; }
            public string Token { get; set; }
            public OldUserInfo(User user, string token) {
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
        OldResult OldLogin(string? id, string? password);
    }
}
