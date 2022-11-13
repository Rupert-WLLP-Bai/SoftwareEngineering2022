using StackExchange.Redis;

namespace SimpleOJTest.ConnectionTest.RedisTest {
    [TestClass]
    public class RedisTest {
        [TestMethod]
        public void ConnectionTest() {
            var redis = ConnectionMultiplexer.Connect("119.3.154.46,password=123456");
            var database = redis.GetDatabase(db:0);
            var redisResult = database.Execute("ping");
            Assert.AreEqual("PONG",redisResult.ToString()!.ToUpper());
        }
    }
}
