using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 测验表
    ///</summary>
    [SugarTable("examination")]
    public class Examination {
        /// <summary>
        /// 测验id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 测验标题 
        ///</summary>
        [SugarColumn(ColumnName = "title")]
        public string? Title { get; set; }
        /// <summary>
        /// 测验描述 
        ///</summary>
        [SugarColumn(ColumnName = "description")]
        public string? Description { get; set; }
        /// <summary>
        /// 测验开始时间 
        ///</summary>
        [SugarColumn(ColumnName = "start_time")]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 测验结束时间 
        ///</summary>
        [SugarColumn(ColumnName = "end_time")]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 测验创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 测验更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 测验发布时间 
        ///</summary>
        [SugarColumn(ColumnName = "distributed_time")]
        public DateTime DistributedTime { get; set; }
        /// <summary>
        /// 测验状态,0为未发布,1为已发布,2为已结束 
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public int Status { get; set; }

        public override string ToString() {
            return
                $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(Description)}: {Description}, {nameof(StartTime)}: {StartTime}, {nameof(EndTime)}: {EndTime}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(DistributedTime)}: {DistributedTime}, {nameof(Status)}: {Status}";
        }
    }
}
