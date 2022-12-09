using System.Collections;
using System.ComponentModel;

namespace SimpleOJ.Common {
    public class Result<T> {
        public bool? Success { get; set; }
        public string? Status { get; set; }
        public int? Code { get; set; }
        public string? Msg { get; set; }
        public T? Data { get; set; }
        public int? Total { get; set; }

        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="status">成功或者失败</param>
        /// <param name="code">状态码</param>
        /// <param name="msg">状态码描述</param>
        /// <param name="data">数据</param>
        /// <param name="total">总数</param>
        private void Init(bool? status, int? code, string? msg, T? data, int? total) {
            Success = status;
            Status = status == true ? "ok" : "error";
            Code = code;
            Msg = msg;
            Data = data;
            Total = total;
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

        public Result(T? data) {
            Init(true, -1, "Uninitialized", data,data is IList list?list.Count:0);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="status">成功或者失败</param>
        /// <param name="resultCode">状态码枚举</param>
        /// <param name="data">数据</param>
        public Result(bool status, ResultCode resultCode, T? data) {
            Init(status, (int)resultCode, GetStatusCodeDescription(resultCode), data, data is IList list ? list.Count : 0);
        }
        public override string ToString() {
            return $"{nameof(Success)}: {Success}, {nameof(Code)}: {Code}, {nameof(Msg)}: {Msg}, {nameof(Data)}: {Data}";
        }
    }

    /// <summary>
    /// 状态码枚举
    /// </summary>
    /// <example> LoginSuccess 1000 </example>
    /// <example> LoginPasswordIncorrect 1001 </example>
    public enum ResultCode {
        // 通用
        [Description("未初始化")] Uninitialized = -1,
        [Description("成功")] Success = 0,
        [Description("失败")] Failure = 1,

        // 登录 10XX
        [Description("登录成功")] LoginSuccess = 1000,
        [Description("登录密码错误")] LoginPasswordIncorrect = 1001,
        [Description("登录账号不存在")] LoginAccountNotExist = 1002,
        [Description("登录账号未激活")] LoginAccountNotActivated = 1003,

        // 注册 11XX
        [Description("注册成功")] RegisterSuccess = 1100,
        [Description("注册账号已存在")] RegisterAccountExist = 1101,
        [Description("数据库添加注册用户失败")] RegisterAddUserFailed = 1102
    }
}
