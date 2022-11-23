using Microsoft.AspNetCore.Mvc;

namespace SimpleOJ.Controllers {

    public interface IFileController {
        //public FileResult Download(string fileId);

        public dynamic Upload(IEnumerable<IFormFile> files, string userId);
    }
}
