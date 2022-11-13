namespace SimpleOJ.Util {
    public class JwtSetting {
        static JwtSetting() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();


            Instance = new JwtSetting
            {
                Audience = config["JwtSetting:Audience"]!,
                SecurityKey = config["JwtSetting:SecurityKey"]!,
                Issuer = config["JwtSetting:Issuer"]!,
                ExpireSeconds = int.Parse(config["JwtSetting:ExpireSeconds"]!)
            };
        }
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpireSeconds { get; set; }
        public static JwtSetting Instance
        {
            get;
        }

    }
}
