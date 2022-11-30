using SimpleOJ.Model;

namespace SimpleOJ.Service {
    public interface IExperimentService {
        /// <summary>
        /// 分页获取实验列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Experiment> GetExperiments(int? pageIndex, int? pageSize,ref int total);

        /// <summary>
        /// 随机生成实验数据
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        bool GenerateExperiments(int count);
    }
}
