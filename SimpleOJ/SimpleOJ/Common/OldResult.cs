using System.ComponentModel;

namespace SimpleOJ.Common {
    public class OldResult {
        public int? Code { get; set; }
        public string? Msg { get; set; }
        public object? Data { get; set; }

        public OldResult() {
            Code = Convert.ToInt32(OldResultCode.Uninitialized);
            Msg = "Uninitialized";
            Data = null;
        }

        /// <summary>
        /// 传入状态码枚举和返回数据，构造统一的返回类
        /// </summary>
        /// <param name="resultEnum">状态码枚举</param>
        /// <param name="data">后端返回的数据</param>
        public OldResult(Enum resultEnum, object? data) {
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
    public enum OldResultCode {
        // 通用状态码

        // 未初始化
        [Description("未初始化")] Uninitialized = -1,
        // 成功
        [Description("成功")] Success = 0,
        // 失败
        [Description("失败")] Failure = 1,

        // 登陆状态码

        // 登陆成功
        [Description("登陆成功")] LoginSuccess = 1000,
        // 登陆用户名不存在
        [Description("登陆用户名不存在")] LoginUsernameNotExist = 1001,
        // 登陆密码错误
        [Description("登陆密码错误")] LoginPasswordError = 1002,

        // 注册状态码

        // 注册成功
        [Description("注册成功")] RegisterSuccess = 2000,
        // 注册id已存在
        [Description("注册id已存在")] RegisterIdExist = 2001,
        // 注册邮箱已存在
        [Description("注册邮箱已存在")] RegisterEmailExist = 2002,
        // 注册手机号已存在
        [Description("注册手机号已存在")] RegisterPhoneExist = 2003,
    }
}
