using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 总成绩表
    ///</summary>
    [SugarTable("total_score")]
    public class TotalScore {
        /// <summary>
        /// 成绩id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 学生id 
        ///</summary>
        [SugarColumn(ColumnName = "student_id")]
        public string? StudentId { get; set; }
        /// <summary>
        /// 成绩 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "score")]
        public int? Score { get; set; }
        /// <summary>
        /// 成绩创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 成绩更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}, {nameof(StudentId)}: {StudentId}, {nameof(Score)}: {Score}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}";
        }
    }
}
