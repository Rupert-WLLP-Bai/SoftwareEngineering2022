using SqlSugar;

namespace SimpleOJ.Model {
    /// <summary>
    /// 实验成绩表
    ///</summary>
    [SugarTable("experiment_score")]
    public class ExperimentScore {
        /// <summary>
        /// 实验成绩id,uuid() 
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
        /// 学生实验成绩 
        ///</summary>
        [SugarColumn(ColumnName = "score")]
        public int? Score { get; set; }
        /// <summary>
        /// 学生实验成绩创建时间 
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 学生实验成绩更新时间 
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        public override string ToString() {
            return
                $"{nameof(Id)}: {Id}, {nameof(ExperimentId)}: {ExperimentId}, {nameof(StudentId)}: {StudentId}, {nameof(Score)}: {Score}, {nameof(CreateTime)}: {CreateTime}, {nameof(UpdateTime)}: {UpdateTime}";
        }
    }
}
