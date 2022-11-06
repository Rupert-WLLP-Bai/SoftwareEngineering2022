using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 实验权重表
    ///</summary>
    [SugarTable("experiment_weight")]
    public class ExperimentWeight {
        /// <summary>
        /// 实验权重id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 实验id 
        ///</summary>
        [SugarColumn(ColumnName = "experiment_id")]
        public string? ExperimentId { get; set; }
        /// <summary>
        /// 实验权重 
        ///</summary>
        [SugarColumn(ColumnName = "weight")]
        public int? Weight { get; set; }
        /// <summary>
        /// 实验权重创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 实验权重更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }
    }
}
