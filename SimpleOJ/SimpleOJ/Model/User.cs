using System.ComponentModel;
using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 用户表
    ///</summary>
    [SugarTable("user")]
    public class User {
        /// <summary>
        /// 用户id,学工号 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 密码 
        ///</summary>
        [SugarColumn(ColumnName = "password")]
        public string? Password { get; set; }
        /// <summary>
        /// 盐值
        /// </summary>
        [SugarColumn(ColumnName = "salt")]
        public string? Salt { get; set; }
        /// <summary>
        /// 姓名 
        ///</summary>
        [SugarColumn(ColumnName = "name")]
        public string? Name { get; set; }
        /// <summary>
        /// 邮箱 
        ///</summary>
        [SugarColumn(ColumnName = "email")]
        public string? Email { get; set; }
        /// <summary>
        /// 电话 
        ///</summary>
        [SugarColumn(ColumnName = "phone")]
        public string? Phone { get; set; }
        /// <summary>
        /// 角色,0为管理员,1为学生,2为教师,3为助教 
        ///</summary>
        [SugarColumn(ColumnName = "role")]
        public int? Role { get; set; }
        /// <summary>
        /// 用户状态,0为禁用,1为正常 
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public int? Status { get; set; }
        /// <summary>
        /// 用户创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 用户更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 用户状态枚举
        /// </summary>
        public enum UserStatus {
            [Description("禁用")]
            Deactivated = 0,
            [Description("正常")]
            Activated = 1
        }
        /// <summary>
        /// 角色枚举
        /// </summary>
        public enum UserRole
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
    }
}
