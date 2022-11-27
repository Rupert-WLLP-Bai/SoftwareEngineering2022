using SimpleOJ.Service;

namespace SimpleOJ.Common {
    public static class Injector {
        public static void RegisterService(this IServiceCollection serviceCollection) {
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IJwtTokenService, JwtTokenService>();
            serviceCollection.AddTransient<IUserLoginService, UserLoginService>();
        }
    }
}
