using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 总权重表
    ///</summary>
    [SugarTable("total_weight")]
    public class TotalWeight {
        /// <summary>
        /// 总权重id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 实验权重 
        ///</summary>
        [SugarColumn(ColumnName = "experiment_weight")]
        public int? ExperimentWeight { get; set; }
        /// <summary>
        /// 测验权重 
        ///</summary>
        [SugarColumn(ColumnName = "examination_weight")]
        public int? ExaminationWeight { get; set; }
        /// <summary>
        /// 总权重创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 总权重更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}, {nameof(ExperimentWeight)}: {ExperimentWeight}, {nameof(ExaminationWeight)}: {ExaminationWeight}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}";
        }
    }
}
