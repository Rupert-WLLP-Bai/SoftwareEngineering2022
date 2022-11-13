using SimpleOJ.Service;

namespace SimpleOJTest.InitializationTest.GenerateInitialData {
    
    public class Generator {
        
        public void Generate() {
            new UserGenerator(new UserService()).GenerateUser();
        }
    }
}
