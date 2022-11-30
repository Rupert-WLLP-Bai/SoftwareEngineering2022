namespace SimpleOJ.Util {
    public static class RandomString {
        public static string Gen(int length) {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(characters, length)
                .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
        }
    }
}
