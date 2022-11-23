using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Util;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : IFileController {
        /*public FileResult Download(string fileId)
        {
            
        }*/
        [Authorize(Roles = "Admin")]
        [HttpPost("Upload")]
        [RequestSizeLimit(1024 * 1024 * 1024)]
        public dynamic Upload(IEnumerable<IFormFile> files, string userId) {
            var f = new List<string>();
            foreach (var file in files) {
                var fileName = file.FileName;
                f.Add(fileName);
                //获取文件名后缀
                var fileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
                //保存文件
                var stream = file.OpenReadStream();
                //把stream转换成byte[]
                var bytes = new byte[stream.Length];
                _ = stream.Read(bytes, 0, bytes.Length);
                //设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);
                //把byte[]写入文件
                var fs = new FileStream(PathSetting.Instance.UserDirectory + "/debug/" + file.FileName, FileMode.Create);
                var bw = new BinaryWriter(fs);
                bw.Write(bytes);
                bw.Close();
                fs.Close();
            }

            return new OldResult(OldResultCode.Success, f);
        }
    }
}
