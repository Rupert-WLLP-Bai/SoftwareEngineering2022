﻿using SimpleOJ.Config;
using SimpleOJ.Model;
using SimpleOJ.Util;
using SqlSugar;

namespace SimpleOJ.Service {
    public class ExperimentService : Repository<Experiment>, IExperimentService {
        public IEnumerable<Experiment> GetExperiments(int? pageIndex, int? pageSize, ref int total) {
            // 如果不需要分页，可以直接调用 base.GetList()
            if (pageIndex == null || pageSize == null) {
                return base.GetList();
            }
            var p = new PageModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var list = Db.Queryable<Experiment>()
                .OrderBy(it => it.Id, OrderByType.Desc)
                .ToPageList(p.PageIndex, p.PageSize, ref total);
            return list;
        }

        public bool GenerateExperiments(int count) {
            for (var i = 0; i < count; i++) {
                var exp = new Experiment()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = RandomString.Gen(20),
                    Status = 0,
                    TeacherId = $"teacher{Random.Shared.Next(3)+1}",
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    DistributeTime = DateTime.Now,
                    FilePath = RandomString.Gen(10),
                    StartTime = DateTime.Today + TimeSpan.FromHours(32),
                    EndTime = DateTime.Today + TimeSpan.FromHours(64),
                    Description = RandomString.Gen(100),
                    UploadTimesLimit = Random.Shared.Next(10)+1,
                };
                base.Insert(exp);
            }
            return true;
        }

        public IEnumerable<Experiment> DeleteExperiments(IEnumerable<string> ids) {
            var list = base.GetList(it => ids.Contains(it.Id));
            foreach (var exp in list) {
                base.Delete(exp);
            }
            return list;
        }

        public Experiment? UploadExperiment(Experiment experiment) {
            // 1. 检查实验是否存在
            var exp = this.GetSingle(experiment1 => experiment1.Id == experiment.Id);
            // 如果实验存在，更新实验
            if (exp != null) {
                exp.Name = experiment.Name;
                exp.Status = experiment.Status;
                exp.TeacherId = experiment.TeacherId;
                exp.UpdateTime = DateTime.Now;
                exp.DistributeTime = experiment.DistributeTime;
                exp.FilePath = experiment.FilePath;
                exp.StartTime = experiment.StartTime;
                exp.EndTime = experiment.EndTime;
                exp.Description = experiment.Description;
                exp.UploadTimesLimit = experiment.UploadTimesLimit;
                base.Update(exp);
            }
            // 如果实验不存在，插入实验
            else {
                experiment.CreateTime = DateTime.Now;
                experiment.UpdateTime = DateTime.Now;
                base.Insert(experiment);
            }
            return experiment;
        }
    }
}
