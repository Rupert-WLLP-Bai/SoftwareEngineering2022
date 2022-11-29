using SimpleOJ.Config;
using SimpleOJ.Model;

namespace SimpleOJ.Service {
    public class UserLoginService : Repository<UserLogin>, IUserLoginService {
        public UserLogin? GetByUserId(string userId) {
            return GetByIdAsync(userId).Result;
        }

        public bool InsertLogin(UserLogin userLogin) {
            return InsertAsync(userLogin).Result;
        }

        public DateTime? GetLastLoginTimeByUserId(string userId) {
            // 不存在则返回null
            return GetByUserId(userId)?.LoginTime;
        }

        public bool UpdateLogin(UserLogin userLogin) {
            // userLogin.UserId用于匹配主键, 更新其他不同的字段
            return this.AsUpdateable(userLogin).UpdateColumns(it => new {it.LoginTime,it.Ip}).ExecuteCommand() > 0;
        }
    }
}
