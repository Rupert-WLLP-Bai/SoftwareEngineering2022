using log4net;
using SqlSugar;

namespace SimpleOJ.Config {
    public class Repository<T> : SimpleClient<T> where T : class, new() {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Repository<T>));
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public Repository(ISqlSugarClient? context = null) : base(context) {
            if (context == null) {
                Context = Db;
            }
        }

        /// <summary>
        /// SqlSugarScope操作数据库是线程安全的可以单例
        /// </summary>
        public static readonly SqlSugarScope Db = new SqlSugarScope(
            new ConnectionConfig
            {
                DbType = DbType.MySql,
                ConnectionString = @"server=119.3.154.46;Database=SE2022;Uid=bjh;Pwd=1230;",
                IsAutoCloseConnection = true
            },
            db => {
                // 输出日志
                db.Aop.OnLogExecuting = (s, p) => {
                    Log.Debug($"{s}\n{string.Join(",", p.Select(a => $"{a.ParameterName}:{a.Value}"))}");
                    
                    // Console.WriteLine(s);
                    // Console.WriteLine(string.Join(",", p.Select(a => a.ParameterName + ":" + a.Value)));
                };
            });
    }
}
