using log4net;
using Microsoft.AspNetCore.Mvc;
using SimpleOJ.Common;
using SimpleOJ.Model;
using SimpleOJ.Service;

namespace SimpleOJ.Controllers {

    [ApiController]
    [Route("api")]
    public class CommonApi : ControllerBase {
        private readonly ILog _log = LogManager.GetLogger(typeof(CommonApi));
        private readonly IUserService _userService;
        private readonly IUserLoginService _userLoginService;
        private readonly IJwtTokenService _jwtTokenService;

        public record CurrentUserInfo(User User, string Token, string? IPv4);

        public CommonApi(IUserService userService, IUserLoginService userLoginService, IJwtTokenService jwtTokenService) {
            _userService = userService;
            _userLoginService = userLoginService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet("CurrentUser")]
        public Result<CurrentUserInfo> GetCurrentUserInfo() {
            var ipv4 = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            // TODO token解析函数有问题
            // 从请求头中获取token
            var token = Request.Headers["Authorization"].ToString()[7..];
            // 从token中获取用户信息
            _log.Debug($"token = {token}");
            string userId;
            try {
                // TODO userId获取不正确
                userId = _jwtTokenService.ParseUserId(token);
            }
            catch {
                _log.Error($"token解析失败, token = {token}");
                return new Result<CurrentUserInfo>(false, ResultCode.Failure, null);
            }

            // [DEBUG]输出
            _log.Debug($"当前用户id为{userId}, token为{token}");
            // 查询用户信息
            var user = _userService.GetByUserId(userId);
            // 如果用户不存在，和超时作同样的处理
            if (user == null) {
                _log.Warn($"用户不存在, 用户id = {userId}");
                return new Result<CurrentUserInfo>(false, ResultCode.Failure, null);
            } else {
                // 用户信息存在
                // 查看是否超过72h
                var lastLoginTime = _userLoginService.GetLastLoginTimeByUserId(user.Id!);
                // 登录信息存在
                // 判断是否超时
                if (lastLoginTime!.Value.AddHours(72) < DateTime.Now) {
                    _log.Info($"用户登录超时, 用户id = {userId}, 跳转到登录页面");
                    return new Result<CurrentUserInfo>(false, ResultCode.Failure, null);
                } else {
                    // 没有超时
                    // 验证Token是否过期
                    var jwtStatus = _jwtTokenService.VerifyToken(token, null);
                    if (jwtStatus == JwtStatus.Expired) {
                        // 返回新token
                        var updateToken = _jwtTokenService.UpdateToken(token);
                        _log.Info($"用户登录成功, 用户token过期, 用户id = {userId}, 返回新token: {updateToken}");
                        return new Result<CurrentUserInfo>(true,
                            ResultCode.Success,
                            new CurrentUserInfo(user, updateToken, ipv4));
                    }

                    // 返回当前token
                    _log.Info($"用户登录成功, 用户id = {userId}, 返回当前token: {token}");
                    return new Result<CurrentUserInfo>(true, ResultCode.Success, new CurrentUserInfo(user, token, ipv4));
                }
            }
        }
    }
}
