﻿using System.Text;
using SimpleOJ.Common;
using SimpleOJ.Util;

namespace SimpleOJ.Service
{

    public interface IJwtTokenService
    {
        /// <summary>
        /// 生成jwtToken
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>jwtToken</returns>
        public string GenerateToken(UserToken userInfo,DateTime now);

        /// <summary>
        /// jetToken延时
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>jwtToken</returns>
        public string UpdateToken(UserToken userInfo);
        
        public string UpdateToken(string token);

        /// <summary>
        /// 判断token携带的user_id是否存在于redis，且token未失效
        /// </summary>
        /// <param name="token"></param>
        /// <returns>JwtToken的解析状态</returns>
        public JwtStatus VerifyToken(string token,string secret);

        public bool TokenStatus(string token, string secret);
        
        /// <summary>
        /// 判断用户(key)是否存在于redis
        /// </summary>
        /// <param name="token"></param>
        /// <returns>bool</returns>
        public bool ContainsKey(string userId);

        /// <summary>
        /// 只解析未过时的token，其余返回exception种类
        /// </summary>
        /// <param name="token"></param>
        /// <returns>未过期token的string类型json，其他情况返回exception</returns>
        public string DeocdeJwtToken(string token,string? key);

    }
}