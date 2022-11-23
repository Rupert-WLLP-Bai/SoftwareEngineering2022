using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 题目提交表
    ///</summary>
    [SugarTable("problem_handin")]
    public class ProblemHandin {
        /// <summary>
        /// 题目提交id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 题目id 
        ///</summary>
        [SugarColumn(ColumnName = "problem_id")]
        public string? ProblemId { get; set; }
        /// <summary>
        /// 学生id 
        ///</summary>
        [SugarColumn(ColumnName = "student_id")]
        public string? StudentId { get; set; }
        /// <summary>
        /// 学生提交代码 
        ///</summary>
        [SugarColumn(ColumnName = "code")]
        public string? Code { get; set; }
        /// <summary>
        /// 学生提交时间 
        ///</summary>
        [SugarColumn(ColumnName = "upload_time")]
        public DateTime UploadTime { get; set; }
        /// <summary>
        /// 学生提交代码语言 
        ///</summary>
        [SugarColumn(ColumnName = "language")]
        public string? Language { get; set; }
        /// <summary>
        /// 学生提交所属测验id,若不属于测验则为null 
        ///</summary>
        [SugarColumn(ColumnName = "examination_id")]
        public string? ExaminationId { get; set; }
        /// <summary>
        /// 学生提交得分 
        ///</summary>
        [SugarColumn(ColumnName = "score")]
        public int? Score { get; set; }

        public override string ToString() {
            return
                $"{nameof(Id)}: {Id}, {nameof(ProblemId)}: {ProblemId}, {nameof(StudentId)}: {StudentId}, {nameof(Code)}: {Code}, {nameof(UploadTime)}: {UploadTime}, {nameof(Language)}: {Language}, {nameof(ExaminationId)}: {ExaminationId}, {nameof(Score)}: {Score}";
        }
    }
}
