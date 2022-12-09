using log4net;
using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Model;
using SimpleOJ.Service;
using SimpleOJ.Common;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ExperimentController : ControllerBase {
        private readonly IExperimentService _experimentService;
        private readonly ILog _logger = LogManager.GetLogger(typeof(ExperimentController));
        public ExperimentController(IExperimentService experimentService) {
            _experimentService = experimentService;
        }
        
        // 分页查询和获取所有实验列表
        public record ExperimentList(IEnumerable<Experiment> Experiments, int Total);
        [HttpGet]
        public Result<ExperimentList> GetExperiments(int? current, int? pageSize) {
            var total = 0;
            var experiments = _experimentService.GetExperiments(current, pageSize, ref total);
            return new Result<ExperimentList>(true,ResultCode.Success, new ExperimentList(experiments, total));
        }
        
        // 批量删除实验
        // 返回删除的实验实体列表
        [HttpDelete]
        public Result<IEnumerable<Experiment>> DeleteExperiments([FromBody] IEnumerable<string> ids) {
            var experiments = _experimentService.DeleteExperiments(ids);
            return new Result<IEnumerable<Experiment>>(true, ResultCode.Success, experiments);
        }
    }
}
