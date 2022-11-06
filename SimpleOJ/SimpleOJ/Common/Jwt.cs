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
        public static string GetToken() {
            //创建用户身份标识，可按需要添加更多信息
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", "2053285", ClaimValueTypes.String), // 用户id
                new Claim("name", "lbj"), // 用户名
                new Claim("admin", true.ToString(), ClaimValueTypes.Boolean) // 是否是管理员
            };
            var key = Encoding.UTF8.GetBytes(JwtSetting.Instance.SecurityKey);
            //创建令牌
            var token = new JwtSecurityToken(
                issuer: JwtSetting.Instance.Issuer,
                audience: JwtSetting.Instance.Audience,
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
}
