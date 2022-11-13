using SimpleOJ.Model;
using SimpleOJ.Service;
using SimpleOJ.Util;

namespace SimpleOJTest.InitializationTest.GenerateInitialData {
    public class UserGenerator {
        private readonly IUserService _userService;

        public UserGenerator(IUserService userService) {
            _userService = userService;
        }

        /// <summary>
        /// 汇总的用户生成函数
        /// </summary>
        public void GenerateUser() {
            AddAdmin();
            AddStudent();
            AddTeacher();
            AddAssistant();
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        public void AddAdmin() {
            var salt = SaltGenerator.GenerateSalt();
            var password = EncryptPassword.Encrypt("admin", salt);
            var admin = new User
            {
                Id = "admin",
                Name = "admin",
                Password = password,
                Salt = salt,
                Email = "admin@gmail.com",
                Phone = "178xxxyyyy",
                Role = (int?)User.UserRole.Admin,
                Status = (int?)User.UserStatus.Activated,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            _userService.AddUser(admin);
        }

        /// <summary>
        /// 添加学生
        /// </summary>
        public void AddStudent() {
            var studentNum = 100;
            for (var i = 0; i < studentNum; i++) {
                var salt = SaltGenerator.GenerateSalt();
                var password = EncryptPassword.Encrypt("student" + i, salt);
                var student = new User
                {
                    Id = "student" + i,
                    Name = "student" + i,
                    Password = password,
                    Salt = salt,
                    Email = "student" + i + "@gmail.com",
                    Phone = "178xxxyyyy",
                    Role = (int?)User.UserRole.Student,
                    Status = (int?)User.UserStatus.Activated,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                _userService.AddUser(student);
            }
        }

        /// <summary>
        /// 添加教师
        /// </summary>
        public void AddTeacher() {
            var teacherNum = 10;
            for (var i = 0; i < teacherNum; i++) {
                var salt = SaltGenerator.GenerateSalt();
                var password = EncryptPassword.Encrypt("teacher" + i, salt);
                var teacher = new User
                {
                    Id = "teacher" + i,
                    Name = "teacher" + i,
                    Password = password,
                    Salt = salt,
                    Email = "teacher" + i + "@gmail.com",
                    Phone = "178xxxyyyy",
                    Role = (int?)User.UserRole.Teacher,
                    Status = (int?)User.UserStatus.Activated,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                _userService.AddUser(teacher);
            }
        }

        /// <summary>
        /// 添加助教
        /// </summary>
        public void AddAssistant() {
            var assistantNum = 50;
            for (var i = 0; i < assistantNum; i++) {
                var salt = SaltGenerator.GenerateSalt();
                var password = EncryptPassword.Encrypt("assistant" + i, salt);
                var assistant = new User
                {
                    Id = "assistant" + i,
                    Name = "assistant" + i,
                    Password = password,
                    Salt = salt,
                    Email = "assistant" + i + "@gmail.com",
                    Phone = "178xxxyyyy",
                    Role = (int?)User.UserRole.Assistant,
                    Status = (int?)User.UserStatus.Activated,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                _userService.AddUser(assistant);
            }
        }

    }
}
