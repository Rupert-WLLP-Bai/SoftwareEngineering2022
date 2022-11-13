using SimpleOJ.Config;
using SimpleOJ.Model;
using SqlSugar;

namespace SimpleOJTest.InitializationTest.GenerateInitialData {
    public static class ClearUser {
        private static readonly Repository<User> UserRepository = new Repository<User>();

        public static void ClearAll() {
            // 删除所有User
            var deleteResult = UserRepository.Delete(it => it.Id != null);
            Console.WriteLine(deleteResult == false ? "User已为空" : "已删除所有用户");
            // 查询User行数，结果为0
            var count = UserRepository.Count(it => it.Id != null);
            Console.WriteLine($"User数量为{count}");
        }
    }
}
