using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJTest.InitializationTest.GenerateInitialData {
    [TestClass]
    public class Generator {
        [TestMethod]
        public void QueryTest() {
            var adminList = new UserService().GetByRole(User.UserRole.Admin);
            // admin的个数为1
            Assert.AreEqual(1, adminList.Count());
        }
    }
}
