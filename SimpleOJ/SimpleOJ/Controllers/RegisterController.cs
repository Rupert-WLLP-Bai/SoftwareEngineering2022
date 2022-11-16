using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;
using SimpleOJ.Util;

namespace SimpleOJ.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController {

        private readonly IUserService _userService;

        public RegisterController() {
            _userService = new UserService();
        }
        
        [HttpPost("Register")]
        public OldResult Register(string id,string password,string name,string email,string phone) {
            // 无权限限制
            // 查询数据库中是否存在id
            var user = _userService.GetByUserId(id);
            if (user != null) {
                // 存在返回null
                return new OldResult(OldResultCode.RegisterIdExist, null);
            }
            // TODO 验证密码的合法性
            // TODO 验证手机邮箱的合法性
            // 添加用户
            var salt = SaltGenerator.GenerateSalt();
            var newUser = new User()
            {
                Id = id,
                Name = name,
                Email = email,
                Phone = phone,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Role = (int?)User.UserRole.Student,
                Salt = salt,
                Password = EncryptPassword.Encrypt(password, salt),
                Status = (int?)User.UserStatus.Activated
            };
            if (_userService.AddUser(newUser) == null) {
                // 添加失败
                return new OldResult(OldResultCode.Failure, null);
            }
            return new OldResult(OldResultCode.Success, newUser);
        }

        //csv表头有要求，表头错误直接爆炸
        [HttpPost("ImportFromCSV")]
        public OldResult ImportFromCSV(IFormFile chart)
        {
            var fileName = chart.FileName;
            string fileExtension = chart.FileName.Substring(chart.FileName.LastIndexOf(".") + 1);//获取文件名后缀
            //保存文件
            var stream = chart.OpenReadStream();
            //把stream转换成byte[]
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes,0,bytes.Length);
            //设置当前流的位置为流的开始
            stream.Seek(0,SeekOrigin.Begin);
            //把byte[]写入文件
            FileStream fs = new FileStream("/Users/cloudwalker/Documents/tmp"+chart.FileName,FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();

            IEnumerable<User> records;
            List<KeyValuePair<User,string>> failures = new List<KeyValuePair<User,string>>();

            using (var reader = new StreamReader("/Users/cloudwalker/Documents/tmp"+chart.FileName))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    records = csv.GetRecords<User>();
                }
            }
            
            
            foreach (var i in records)
            {
                var user = _userService.GetByUserId(i.Id);
                if (user != null) {
                    // 存在返回null
                    failures.Add(new KeyValuePair<User, string>(i,"alreadyAdded"));
                    continue;
                }
                // TODO 验证密码的合法性
                // TODO 验证手机邮箱的合法性
                // 添加用户
                var salt = SaltGenerator.GenerateSalt();
                var newUser = new User()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Email = i.Email,
                    Phone = i.Phone,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    Role = (int?)User.UserRole.Student,
                    Salt = salt,
                    Password = EncryptPassword.Encrypt(i.Password!, salt),
                    Status = (int?)User.UserStatus.Activated
                };
                if (_userService.AddUser(newUser) == null) {
                    // 添加失败
                    failures.Add(new KeyValuePair<User, string>(i,"add Failed"));
                }
            }
            //var reader = new StreamReader("/Users/cloudwalker/Documents/tmp"+chart.FileName);

            //var csv = new CsvReader(reader,CultureInfo.InvariantCulture);

            //var records = csv.GetRecords<User>();

            return new OldResult(OldResultCode.Success,failures);//返回未插入的表项
        }
    }
}                   
