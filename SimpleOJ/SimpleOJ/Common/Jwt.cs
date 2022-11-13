using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.IdentityModel.Tokens;
using SimpleOJ.Util;

namespace SimpleOJ.Common {
    public class Jwt {
        public static string GetToken(UserToken userInfo) {
            //创建用户身份标识，可按需要添加更多信息
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//jwt id 
                new Claim("id", userInfo.Id, ClaimValueTypes.String), // 用户id
                new Claim("role", userInfo.Role.ToString(), ClaimValueTypes.Integer) // 用户权限
            };
            
            var key = Encoding.UTF8.GetBytes(JwtSetting.Instance.SecurityKey);
            //创建令牌
            var token = new JwtSecurityToken(
                issuer: JwtSetting.Instance.Issuer,
                audience: userInfo.Id,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(JwtSetting.Instance.ExpireSeconds) //加载自配置文件appsetting.json
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        public static string VerifyToken(string token, string secret) {
            try {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm alg = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
                var json = decoder.Decode(token, secret, true);
                //校验通过，返回解密后的字符串
                return json;
            }
            catch (TokenExpiredException) {
                //表示过期
                return "expired";
            }
            catch (SignatureVerificationException) {
                //表示验证不通过
                return "invalid";
            }
            catch (Exception) {
                return "error";
            }
        }
    }
    /// <summary>
    /// Jwt状态枚举
    /// </summary>
    public enum JwtStatus
    {
        [Description("有效")]
        Valid = 0,
        [Description("过期")]
        Expired = 1,
        [Description("无效")]
        Invalid = 2,//错误token
        [Description("错误")]
        Error = 3,//解析失败

    }
}
