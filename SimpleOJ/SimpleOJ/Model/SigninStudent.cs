using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 签到学生表
    ///</summary>
    [SugarTable("signin_student")]
    public class SigninStudent {
        /// <summary>
        /// 签到id 
        ///</summary>
        [SugarColumn(ColumnName = "signin_id", IsPrimaryKey = true)]
        public string? SigninId { get; set; }
        /// <summary>
        /// 学生id 
        ///</summary>
        [SugarColumn(ColumnName = "student_id", IsPrimaryKey = true)]
        public string? StudentId { get; set; }
        /// <summary>
        /// 签到学生创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 签到学生更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        public override string ToString() {
            return
                $"{nameof(SigninId)}: {SigninId}, {nameof(StudentId)}: {StudentId}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}";
        }
    }
}
