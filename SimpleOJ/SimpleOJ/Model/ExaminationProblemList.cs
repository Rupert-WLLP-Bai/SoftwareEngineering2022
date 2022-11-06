using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 测验题目列表
    ///</summary>
    [SugarTable("examination_problem_list")]
    public class ExaminationProblemList {
        /// <summary>
        /// 测验id 
        ///</summary>
        [SugarColumn(ColumnName = "examination_id", IsPrimaryKey = true)]
        public string? ExaminationId { get; set; }
        /// <summary>
        /// 题目id 
        ///</summary>
        [SugarColumn(ColumnName = "problem_id", IsPrimaryKey = true)]
        public string? ProblemId { get; set; }
    }
}
