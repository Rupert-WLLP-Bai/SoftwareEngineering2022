using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 测验学生列表
    ///</summary>
    [SugarTable("examination_student_list")]
    public class ExaminationStudentList {
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
    }
}
