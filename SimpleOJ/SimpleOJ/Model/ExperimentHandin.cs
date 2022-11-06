using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 实验提交表
    ///</summary>
    [SugarTable("experiment_handin")]
    public class ExperimentHandin {
        /// <summary>
        /// 实验提交id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 实验id 
        ///</summary>
        [SugarColumn(ColumnName = "experiment_id")]
        public string? ExperimentId { get; set; }
        /// <summary>
        /// 学生id 
        ///</summary>
        [SugarColumn(ColumnName = "student_id")]
        public string? StudentId { get; set; }
        /// <summary>
        /// 实验提交文件路径 
        ///</summary>
        [SugarColumn(ColumnName = "file_path")]
        public string? FilePath { get; set; }
        /// <summary>
        /// 实验提交创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 实验提交更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }
    }
}
