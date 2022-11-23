namespace SimpleOJ.Util {
    public class PathSetting {
        static PathSetting() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("path.json");

            var config = builder.Build();


            Instance = new PathSetting
            {
                Root = config["Root"]!,
                UserDirectory = config["UserDirectory"]!,
                JudgerDirectory = config["Judger"]!
            };
        }
        public string Root { get; set; }
        public string UserDirectory { get; set; }
        public string JudgerDirectory { get; set; }


        public static PathSetting Instance
        {
            get;
        }

    }
}
