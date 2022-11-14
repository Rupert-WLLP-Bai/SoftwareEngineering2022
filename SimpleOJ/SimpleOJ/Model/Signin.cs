using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 签到表
    ///</summary>
    [SugarTable("signin")]
    public class Signin {
        /// <summary>
        /// 签到id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 教师id 
        ///</summary>
        [SugarColumn(ColumnName = "teacher_id")]
        public string? TeacherId { get; set; }
        /// <summary>
        /// 签到名称 
        ///</summary>
        [SugarColumn(ColumnName = "name")]
        public string? Name { get; set; }
        /// <summary>
        /// 签到开始时间 
        ///</summary>
        [SugarColumn(ColumnName = "start_time")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 签到结束时间 
        ///</summary>
        [SugarColumn(ColumnName = "end_time")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 签到创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 签到更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}, {nameof(TeacherId)}: {TeacherId}, {nameof(Name)}: {Name}, {nameof(StartTime)}: {StartTime}, {nameof(EndTime)}: {EndTime}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}";
        }
    }
}
