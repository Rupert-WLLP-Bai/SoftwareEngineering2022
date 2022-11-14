using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Http;
using SimpleOJ.Common;
using SimpleOJ.Controllers;
using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJTest {
    [TestClass]
    public class LoginTest {
        [TestInitialize]
        public void Init() {
            // 读取log4net配置
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }
        private readonly IUserService _userService;
        private static readonly ILog Log = LogManager.GetLogger("LoginTest");
        public LoginTest() {
            _userService = new UserService();
        }
        /// <summary>
        /// 模拟管理员登录
        /// </summary>
        [TestMethod]
        public void AdminLogin() {
            var admins = _userService.GetByRole(User.UserRole.Admin) as List<User>;
            // 存在管理员
            Assert.IsNotNull(admins);
            Assert.AreNotEqual(0, admins.Count);
            // 随机取出一个管理员
            var admin = admins.OrderBy(_=>Guid.NewGuid()).First();
            Assert.IsNotNull(admin);
            // TODO 测试登录
            var result = new OldOldLoginController().OldLogin("admin","admin");
            Assert.AreEqual(Convert.ToInt32(OldResultCode.LoginSuccess),result.Code);
            Log.Debug(result.Msg);
        }

        [TestMethod]
        public void NewLoginTest() {
            ILoginController loginController = new LoginController(new HttpContextAccessor());
            var status = loginController.Login("admin","admin").Status;
            Assert.AreEqual(true,status);
            for (var i = 0; i < 10; i++) {
                var s = loginController.Login($"student{i}", $"student{i}").Status;
                Assert.AreEqual(true,s);
            }

            for (var i = 0; i < 5; i++) {
                var s1 = loginController.Login($"teacher{i}", $"teacher{i}").Status;
                var s2 = loginController.Login($"assistant{i}", $"assistant{i}").Status;
                Assert.AreEqual(true,s1);
                Assert.AreEqual(true,s2);
            }
        }
    }
}
