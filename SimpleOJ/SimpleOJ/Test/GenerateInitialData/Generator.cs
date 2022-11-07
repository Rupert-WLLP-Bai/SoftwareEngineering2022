using Xunit;

namespace SimpleOJ.Test.GenerateInitialData {
    public class Generator {
        [Fact]
        public void Generate() {
            new UserGenerator().GenerateUser();
        }
    }
}
