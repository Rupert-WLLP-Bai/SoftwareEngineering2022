using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;

namespace SimpleOJ.Controllers {

    public interface IFileController {
        //public FileResult Download(string fileId);

        public Result<IEnumerable<string>> Upload(IEnumerable<IFormFile> files, string userId);
    }
}
