using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 用户登录表
    ///</summary>
    [SugarTable("user_login")]
    public class UserLogin {
        /// <summary>
        /// 用户id 
        ///</summary>
        [SugarColumn(ColumnName = "user_id", IsPrimaryKey = true)]
        public string? UserId { get; set; }
        /// <summary>
        /// 用户登录时间 
        ///</summary>
        [SugarColumn(ColumnName = "login_time", IsPrimaryKey = true)]
        public DateTime? LoginTime { get; set; }
        /// <summary>
        /// 用户登录ip 
        ///</summary>
        [SugarColumn(ColumnName = "ip")]
        public string? Ip { get; set; }
    }
}
