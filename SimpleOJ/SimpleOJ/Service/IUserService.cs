using System.ComponentModel;
using SimpleOJ.Model;

namespace SimpleOJ.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="user"></param>
        /// <returns>用户列表</returns>
        public IEnumerable<User> GetList(User user);

        /// <summary>
        /// 角色枚举
        /// </summary>
        public enum Role
        {
            [Description("管理员")]
            Admin = 0,
            [Description("学生")]
            Student = 1,
            [Description("教师")]
            Teacher = 2,
            [Description("助教")]
            Assistant = 3
        }
        /// <summary>
        /// 根据角色查询
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns>对应角色用户列表</returns>
        public IEnumerable<User> GetByRole(Role role);
    }
}
