namespace SimpleOJ.Util {
    public class SaltGenerator {
        /// <summary>
        /// 生成盐
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt() {
            // TODO 盐的生成还可以改进
            return Guid.NewGuid().ToString("N");
        }
    }
}
