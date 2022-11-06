using SimpleOJ.Config;
using SimpleOJ.Model;

namespace SimpleOJ.Service {
    public class UserService : Repository<User>, IUserService {
        public IEnumerable<User> GetList(User user) {
            return this.GetList(it =>
                it.Id!.Equals(user.Id ?? it.Id) &&
                it.Password!.Equals(user.Password ?? it.Password) &&
                it.Name!.Equals(user.Name ?? it.Name) &&
                it.Email!.Equals(user.Email ?? it.Email) &&
                it.Phone!.Equals(user.Phone ?? it.Phone) &&
                it.Role! == (user.Role ?? it.Role) &&
                it.Status! == (user.Status ?? it.Status) &&
                it.CreateTime!.Equals(user.CreateTime ?? it.CreateTime) &&
                it.UpdateTime!.Equals(user.UpdateTime ?? it.UpdateTime)
            );
        }

        public IEnumerable<User> GetByRole(IUserService.Role role) {
            return this.GetList(it => it.Role! == (int?)role);
        }
    }
}
