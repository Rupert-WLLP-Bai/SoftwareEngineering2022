using SimpleOJ.Model;

namespace SimpleOJ.Service {
    public interface IUserLoginService {
        /// <summary>
        /// 用Id获取用户登录情况
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户登录情况</returns>
        public UserLogin? GetByUserId(string userId);

        /// <summary>
        /// 插入一条登录记录
        /// </summary>
        /// <param name="userLogin">用户登录记录</param>
        /// <returns></returns>
        public bool InsertLogin(UserLogin userLogin);

        /// <summary>
        /// 用Id获取上一次登录时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DateTime? GetLastLoginTimeByUserId(string userId);
        
        /// <summary>
        /// 更新用户登录状态
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public bool UpdateLogin(UserLogin userLogin);
    }
}
