using SimpleOJ.Service;

namespace SimpleOJTest.InitializationTest.GenerateInitialData {
    [TestClass]
    public class Main {
        [TestMethod]
        public void ExpGen() {
            new ExperimentService().GenerateExperiments(100);
        }
    }
}
