using SimpleOJ.Common;

namespace SimpleOJ.Controllers {
    public interface ILoginController {
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="password">用户密码</param>
        /// <returns>
        ///     <example>登录成功，返回用户信息和token</example>
        ///     <example>登录失败，返回错误信息</example>
        /// </returns>
        Result Login(string? id, string? password);
    }
}
