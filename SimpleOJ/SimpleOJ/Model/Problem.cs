using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 题目表
    ///</summary>
    [SugarTable("problem")]
    public class Problem {
        /// <summary>
        /// 题目id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 题目标题 
        ///</summary>
        [SugarColumn(ColumnName = "title")]
        public string? Title { get; set; }
        /// <summary>
        /// 题目描述 
        ///</summary>
        [SugarColumn(ColumnName = "description")]
        public string? Description { get; set; }
        /// <summary>
        /// 题目标签 
        ///</summary>
        [SugarColumn(ColumnName = "tag")]
        public string? Tag { get; set; }
        /// <summary>
        /// 题目样例输入 
        ///</summary>
        [SugarColumn(ColumnName = "sample_input")]
        public string? SampleInput { get; set; }
        /// <summary>
        /// 题目样例输出 
        ///</summary>
        [SugarColumn(ColumnName = "sample_output")]
        public string? SampleOutput { get; set; }
        /// <summary>
        /// 题目样例解释 
        ///</summary>
        [SugarColumn(ColumnName = "sample_explaination")]
        public string? SampleExplaination { get; set; }
        /// <summary>
        /// 题目难度,0为Easy,1为Medium,2为Hard 
        ///</summary>
        [SugarColumn(ColumnName = "difficulty")]
        public int? Difficulty { get; set; }
        /// <summary>
        /// 题目创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 题目更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 题目代码模板 
        ///</summary>
        [SugarColumn(ColumnName = "code_template")]
        public string? CodeTemplate { get; set; }
        /// <summary>
        /// 题目测试用例 
        ///</summary>
        [SugarColumn(ColumnName = "test_case")]
        public string? TestCase { get; set; }
    }
}
