using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using ServiceStack.Redis;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Util;

namespace SimpleOJ.Service {

    public class JwtTokenService : IJwtTokenService {
        public static RedisClient client = new RedisClient("150.158.80.33", 6379, "sadse");

        public string GenerateToken(UserToken userInfo, DateTime now) {

            var key = Encoding.UTF8.GetBytes(JwtSetting.Instance.SecurityKey);

            //var now = DateTime.Now;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //jwt id 
                new Claim("id", userInfo.Id, ClaimValueTypes.String), // 用户id
                new Claim("role", userInfo.Role.ToString(), ClaimValueTypes.Integer), // 用户权限
                new Claim(ClaimTypes.Role, userInfo.Role.ToString(), ClaimValueTypes.Integer), // 用户权限 (Authorize)
                new Claim("issuedAt", now.ToLocalTime().ToString(CultureInfo.CurrentCulture), ClaimValueTypes.DateTime)
            };
            //创建令牌
            var token = new JwtSecurityToken(
                issuer: JwtSetting.Instance.Issuer,
                audience: userInfo.Id,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                notBefore: now,
                expires: now.AddSeconds(JwtSetting.Instance.ExpireSeconds) //加载自配置文件appsetting.json
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            //Set方法可以覆盖原有key值条目对应的键值
            //
            client.Set(userInfo.Id,
                now.ToLocalTime().ToString(CultureInfo.CurrentCulture),
                new TimeSpan(0, 0, 30, 0)); //refresh token存入redis
            return jwtToken;
        }

        public void DeleteToken(string userId) {
            //token直接过期
            client.Set(userId, DateTime.Now, new TimeSpan(0, 0, 0, 1));
        }

        public string UpdateToken(UserToken userInfo) {
            var now = DateTime.Now;
            //refresh token
            //无条目时创建，有条目时覆盖
            client.Set(userInfo.Id, now.ToLocalTime().ToString(CultureInfo.CurrentCulture), new TimeSpan(0, 0, 30, 0));
            //access token
            return GenerateToken(userInfo, now);
            /*var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//jwt id 
                new Claim("id", userInfo.Id, ClaimValueTypes.String), // 用户id
                new Claim("role", userInfo.Role.ToString(), ClaimValueTypes.Integer), // 用户权限
                new Claim("issuedAt",now.ToFileTimeUtc().ToString(),ClaimValueTypes.DateTime)
            };
            
            var key = Encoding.UTF8.GetBytes(JwtSetting.Instance.SecurityKey);
            
            //创建令牌
            var token = new JwtSecurityToken(
                issuer: JwtSetting.Instance.Issuer,
                audience: userInfo.Id,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                notBefore: now,
                expires: now.AddSeconds(JwtSetting.Instance.ExpireSeconds) //加载自配置文件appsetting.json
            );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;*/
        }

        public string UpdateToken(string token) {
            var res = VerifyToken(token, null);
            switch (res) {
                case JwtStatus.Expired:
                {
                    var claims = parseJwt(token, null);
                    var userId = claims["id"].ToString();
                    var role = claims["role"].ToString();
                    var userInfo = new UserToken(userId, User.UserRole.Student);

                    var now = DateTime.Now;
                    client.Set(userId, now, new TimeSpan(0, 0, 30, 0));
                    return GenerateToken(userInfo, now);
                }


            }

            return "";
        }

        public JwtStatus VerifyToken(string token, string? secret) {
            var json = DecodeJwtToken(token, secret);
            switch (json) {
                case "error":
                {
                    return JwtStatus.Error;
                }
                case "invalid":
                {
                    return JwtStatus.Invalid;
                }
                case "expired":
                {
                    var claims = parseJwt(token, secret);
                    if (claims == null) {
                        return JwtStatus.Invalid;
                    }

                    var userId = claims["id"].ToString();
                    var issuedAt = claims["issuedAt"].ToString();
                    if (ContainsKey(userId) && issuedAt.Equals(client.Get<string>(userId))) {
                        return JwtStatus.Expired;
                    }

                    return JwtStatus.Invalid;
                }
                default:
                {
                    var jObject = JObject.Parse(json.ToString());
                    var userId = jObject["id"].ToString();
                    var issuedAt = jObject["issuedAt"].ToString();
                    if (ContainsKey(userId) && issuedAt.Equals(client.Get<string>(userId))) {
                        return JwtStatus.Valid;
                    }

                    return JwtStatus.Invalid;
                }
            }

            return JwtStatus.Error;
        }

        public bool TokenStatus(string token, string secret) {
            try {
                secret = secret ?? JwtSetting.Instance.SecurityKey;
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm alg = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
                var json = decoder.Decode(token, secret, true);
                return true;
                //校验通过,token未过期
            }
            catch (TokenExpiredException e) {
                //表示过期
                return false;
            }
        }

        public string ParseUserId(string token) {
            var secret = JwtSetting.Instance.SecurityKey;

            try {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm alg = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
                var json = decoder.Decode(token, secret, true);
                JObject jo = JObject.Parse(json);
                return jo["id"].ToString();
            }
            catch (TokenExpiredException e) {
                //表示过期
                var claims = e.PayloadData;
                //表示过期
                return claims["id"].ToString();
            }
            catch (SignatureVerificationException) {
                //表示验证不通过
                return "invalid";
            }
            catch (Exception) {
                return "error";
            }
        }

        public bool ContainsKey(string userId) {
            return client.ContainsKey(userId);
        }

        public IReadOnlyDictionary<string, object> parseJwt(String token, string? secret) {
            try {
                secret = secret ?? JwtSetting.Instance.SecurityKey;
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm alg = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
                var json = decoder.Decode(token, secret, true);

                //校验通过，返回解密后的字符串
            }
            catch (TokenExpiredException e) {
                var claims = e.PayloadData;
                //表示过期
                return claims;
            }

            return null;
        }




        public object DecodeJwtToken(string token, string? key) {
            var secret = key ?? JwtSetting.Instance.SecurityKey;

            try {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm alg = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
                var json = decoder.Decode(token, secret, true);
                JObject jo = JObject.Parse(json);
                return jo["role"].ToString();
                //校验通过，返回解密后的字符串
                //return json;
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

            return null;
        }
    }
}
