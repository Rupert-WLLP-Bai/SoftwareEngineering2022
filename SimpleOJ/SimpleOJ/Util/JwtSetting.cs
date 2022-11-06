namespace SimpleOJ.Util {
    public class JwtSetting {
        static JwtSetting() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot Config = builder.Build();


            Instance = new JwtSetting();
            Instance.Audience = Config["JwtSetting:Audience"];
            Instance.SecurityKey = Config["JwtSetting:SecurityKey"];
            Instance.Issuer = Config["JwtSetting:Issuer"];
            Instance.ExpireSeconds = int.Parse(Config["JwtSetting:ExpireSeconds"]);
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
