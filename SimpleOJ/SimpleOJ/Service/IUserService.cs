using SimpleOJ.Model;

namespace SimpleOJ.Service {
    public interface IUserService {
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns>用户列表</returns>
        public IEnumerable<User> GetList(User user);

        /// <summary>
        /// 根据角色查询
        /// </summary>
        /// <param name="userRole">角色</param>
        /// <returns>对应角色用户列表</returns>
        public IEnumerable<User> GetByRole(User.UserRole userRole);

        /// <summary>
        /// 使用id获取用户
        /// </summary>
        /// <param name="id">学工号</param>
        /// <returns>返回对应用户，不存在则返回空用户</returns>
        public User? GetByUserId(string? id);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">新用户</param>
        /// <returns>成功返回用户，不成功返回null</returns>
        public User? AddUser(User user);

        /// <summary>
        /// 分页获取用户
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<User> GetUsers(int? pageIndex, int? pageSize);
    }
}
