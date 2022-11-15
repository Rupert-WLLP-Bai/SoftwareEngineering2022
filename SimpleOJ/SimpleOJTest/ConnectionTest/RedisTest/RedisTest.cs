using log4net;
using log4net.Config;
using ServiceStack.Redis;
using StackExchange.Redis;

namespace SimpleOJTest.ConnectionTest.RedisTest {
    [TestClass]
    public class RedisTest {
        [TestInitialize]
        public void Init() {
            // 读取log4net配置
            // XmlConfigurator.Configure(new FileInfo("log4net.properties"));
        }
        private static readonly ILog Log = LogManager.GetLogger("RedisTest");
        [TestMethod]
        public void ConnectionTest() {
            // redis - 119.3.154.46 - huawei
            Log.Info("Redis 119.3.154.46测试");
            var redis = ConnectionMultiplexer.Connect("119.3.154.46,password=123456");
            var database = redis.GetDatabase(db: 0);
            var redisResult = database.Execute("ping");
            Assert.AreEqual("PONG", redisResult.ToString()!.ToUpper());

            // redis - 150.158.80.33
            Log.Info("Redis 150.158.80.33测试");
            var redisClient = new RedisClient("150.158.80.33", 6379, "sadse");
            var ping = redisClient.Ping();
            Assert.AreEqual(true, ping);
        }
    }
}
