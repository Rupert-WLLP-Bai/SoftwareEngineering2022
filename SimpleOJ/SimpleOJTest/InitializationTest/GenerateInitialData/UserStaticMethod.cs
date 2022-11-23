using SimpleOJ.Config;
using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJTest.InitializationTest.GenerateInitialData {
    public static class UserStaticMethod {
        private static readonly Repository<User> UserRepository = new Repository<User>();

        /// <summary>
        /// 删除所有User
        /// </summary>
        public static void ClearAll() {
            // 删除所有User
            var deleteResult = UserRepository.Delete(it => it.Id != null);
            Console.WriteLine(deleteResult == false ? "User已为空" : "已删除所有用户");
            // 查询User行数，结果为0
            var count = UserRepository.Count(it => it.Id != null);
            Console.WriteLine($"User数量为{count}");
        }

        public static void Generate() {
            var count = new Repository<User>().Count(it => it.Id != null);
            if (count != 0) {
                Console.WriteLine("执行生成User前User表不为空，清空User表");
                ClearAll();
            }

            new UserGenerator(new UserService()).GenerateUser();
        }
    }
}
