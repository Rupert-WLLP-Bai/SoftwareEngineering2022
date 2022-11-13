using SimpleOJ.Config;
using SimpleOJ.Model;

namespace SimpleOJ.Service {
    public class UserService : Repository<User>, IUserService {
        public IEnumerable<User> GetList(User user) {
            return this.GetList(it =>
                it.Id!.Equals(user.Id ?? it.Id) &&
                it.Password!.Equals(user.Password ?? it.Password) &&
                it.Salt!.Equals(user.Salt ?? it.Salt) &&
                it.Name!.Equals(user.Name ?? it.Name) &&
                it.Email!.Equals(user.Email ?? it.Email) &&
                it.Phone!.Equals(user.Phone ?? it.Phone) &&
                it.Role! == (user.Role ?? it.Role) &&
                it.Status! == (user.Status ?? it.Status) &&
                it.CreateTime!.Equals(user.CreateTime ?? it.CreateTime) &&
                it.UpdateTime!.Equals(user.UpdateTime ?? it.UpdateTime)
            );
        }

        public IEnumerable<User> GetByRole(User.UserRole userRole) {
            return this.GetList(it => it.Role! == (int?)userRole);
        }

        public User? GetByUserId(string? id) {
            return this.GetById(id);
        }

        public User? AddUser(User user) {
            return this.Insert(user) ? user : null;
        }
    }
}
