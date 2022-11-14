using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 实验表
    ///</summary>
    [SugarTable("experiment")]
    public class Experiment {
        /// <summary>
        /// 实验id,uuid() 
        ///</summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public string? Id { get; set; }
        /// <summary>
        /// 教师id 
        ///</summary>
        [SugarColumn(ColumnName = "teacher_id")]
        public string? TeacherId { get; set; }
        /// <summary>
        /// 实验名称 
        ///</summary>
        [SugarColumn(ColumnName = "name")]
        public string? Name { get; set; }
        /// <summary>
        /// 实验描述 
        ///</summary>
        [SugarColumn(ColumnName = "description")]
        public string? Description { get; set; }
        /// <summary>
        /// 实验文件路径,没有文件则为null 
        ///</summary>
        [SugarColumn(ColumnName = "file_path")]
        public string? FilePath { get; set; }
        /// <summary>
        /// 实验状态,0为未发布,1为已发布,2为已结束 
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public int? Status { get; set; }
        /// <summary>
        /// 实验创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 实验更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 实验发布时间,未发布则为空 
        ///</summary>
        [SugarColumn(ColumnName = "distribute_time")]
        public DateTime? DistributeTime { get; set; }
        /// <summary>
        /// 实验开始时间,未发布则为空 
        ///</summary>
        [SugarColumn(ColumnName = "start_time")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 实验截止时间,未发布则为空 
        ///</summary>
        [SugarColumn(ColumnName = "end_time")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 实验提交次数限制,未发布则为空 
        ///</summary>
        [SugarColumn(ColumnName = "upload_times_limit")]
        public int? UploadTimesLimit { get; set; }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}, {nameof(TeacherId)}: {TeacherId}, {nameof(Name)}: {Name}, {nameof(Description)}: {Description}, {nameof(FilePath)}: {FilePath}, {nameof(Status)}: {Status}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}, {nameof(DistributeTime)}: {DistributeTime}, {nameof(StartTime)}: {StartTime}, {nameof(EndTime)}: {EndTime}, {nameof(UploadTimesLimit)}: {UploadTimesLimit}";
        }
    }
}
