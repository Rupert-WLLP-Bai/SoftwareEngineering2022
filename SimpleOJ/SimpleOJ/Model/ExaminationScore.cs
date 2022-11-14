using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 测验成绩表
    ///</summary>
    [SugarTable("examination_score")]
    public class ExaminationScore {
        /// <summary>
        /// 测验id 
        ///</summary>
        [SugarColumn(ColumnName = "examination_id", IsPrimaryKey = true)]
        public string? ExaminationId { get; set; }
        /// <summary>
        /// 学生id 
        ///</summary>
        [SugarColumn(ColumnName = "student_id", IsPrimaryKey = true)]
        public string? StudentId { get; set; }
        /// <summary>
        /// 学生测验成绩 
        ///</summary>
        [SugarColumn(ColumnName = "score")]
        public int? Score { get; set; }
        /// <summary>
        /// 学生测验成绩创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 学生测验成绩更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        public override string ToString() {
            return $"{nameof(ExaminationId)}: {ExaminationId}, {nameof(StudentId)}: {StudentId}, {nameof(Score)}: {Score}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}";
        }
    }
}
