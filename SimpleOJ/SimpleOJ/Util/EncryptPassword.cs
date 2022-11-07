using System.Security.Cryptography;
using System.Text;

namespace SimpleOJ.Util {
    /// <summary>
    /// 加密密码
    /// </summary>
    public static class EncryptPassword {
        // 使用SHA256加密密码
        private static readonly SHA256 Sha256 = SHA256.Create();

        public static string Encrypt(string password, string salt) {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            var passwordAndSaltBytes = passwordBytes.Concat(saltBytes).ToArray();
            var hashBytes = Sha256.ComputeHash(passwordAndSaltBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
