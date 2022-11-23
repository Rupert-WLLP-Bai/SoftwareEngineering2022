using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 测验权重表
    ///</summary>
    [SugarTable("examination_weight")]
    public class ExaminationWeight {
        /// <summary>
        /// 测验权重id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 测验id 
        ///</summary>
        [SugarColumn(ColumnName = "examination_id")]
        public string? ExaminationId { get; set; }
        /// <summary>
        /// 测验权重 
        ///</summary>
        [SugarColumn(ColumnName = "weight")]
        public int? Weight { get; set; }
        /// <summary>
        /// 测验权重创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 测验权重更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        public override string ToString() {
            return
                $"{nameof(Id)}: {Id}, {nameof(ExaminationId)}: {ExaminationId}, {nameof(Weight)}: {Weight}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}";
        }
    }
}
