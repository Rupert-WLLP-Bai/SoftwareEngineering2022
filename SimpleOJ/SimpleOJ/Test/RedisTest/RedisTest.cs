using StackExchange.Redis;
using Xunit;
using Xunit.Abstractions;

namespace SimpleOJ.Test.RedisTest {
    public class RedisTest {
        private readonly ITestOutputHelper _testOutputHelper;

        public RedisTest(ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
        }
        [Fact]
        public void ConnectionTest() {
            var redis = ConnectionMultiplexer.Connect("119.3.154.46,password=123456");
            var database = redis.GetDatabase(db:0);
            var redisResult = database.Execute("ping");
            Assert.Equal("PONG",redisResult.ToString()!.ToUpper());
        }
    }
}
