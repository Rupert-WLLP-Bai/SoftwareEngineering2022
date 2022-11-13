using log4net;
using log4net.Config;
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
            Assert.AreNotEqual(0,admins.Count());
            // 随机取出一个管理员
            var admin = admins.OrderBy(it=>Guid.NewGuid()).First();
            Assert.IsNotNull(admin);
            // TODO 测试登录
            var result = new LoginController().Login("admin","admin");
            Assert.AreEqual(Convert.ToInt32(ResultCode.LoginSuccess),result.Code);
            Log.Debug(result.Msg);
        }
    }
}
