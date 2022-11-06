using System.ComponentModel;

namespace SimpleOJ.Common {
    public class Result {
        public int? Code { get; set; }
        public string? Msg { get; set; }
        public object? Data { get; set; }

        public Result() {
            Code = Convert.ToInt32(ResultCode.Uninitialized);
            Msg = "Uninitialized";
            Data = null;
        }

        /// <summary>
        /// 传入状态码枚举和返回数据，构造统一的返回类
        /// </summary>
        /// <param name="resultEnum">状态码枚举</param>
        /// <param name="data">后端返回的数据</param>
        public Result(Enum resultEnum, object? data) {
            Code = Convert.ToInt32(resultEnum);
            Msg = GetStatusCodeDescription(resultEnum);
            Data = data;
        }

        /// <summary>
        /// 获取状态码的描述
        /// </summary>
        /// <param name="en">状态码枚举</param>
        /// <returns></returns>
        private static string GetStatusCodeDescription(Enum en) {
            var type = en.GetType(); //获取类型  
            var memberInfos = type.GetMember(en.ToString()); //获取成员  
            if (memberInfos.Length <= 0) return en.ToString();
            if (memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[]
                {
                    Length: > 0
                } attrs) {
                return attrs[0].Description; //返回当前描述
            }
            return en.ToString();
        }
    }

    /// <summary>
    /// 状态码枚举
    /// </summary>
    public enum ResultCode {
        // 未初始化
        [Description("未初始化")] Uninitialized = -1,
        // 成功
        [Description("成功")] Success = 0,
        // 失败
        [Description("失败")] Failure = 1,
        // 未登录
        [Description("未登录")] NotLogin = 2,
        // 未授权
        [Description("未授权")] NotAuthorized = 3,
    }
}
