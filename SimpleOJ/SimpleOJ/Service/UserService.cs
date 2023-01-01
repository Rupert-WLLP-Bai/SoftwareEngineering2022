using SimpleOJ.Config;
using SimpleOJ.Model;
using SqlSugar;

namespace SimpleOJ.Service {
    public class UserService : Repository<User>, IUserService {
        public IEnumerable<User> GetList(User user) {
            return GetList(it =>
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
            return GetList(it => it.Role! == (int?)userRole);
        }

        public User? GetByUserId(string? id) {
            return GetById(id);
        }

        public User? AddUser(User user) {
            return Insert(user) ? user : null;
        }

        public IEnumerable<User> GetUsers(int? pageIndex, int? pageSize) {
            // 如果不需要分页，可以直接调用 base.GetList()
            if (pageIndex == null || pageSize == null) {
                return base.GetList();
            }
            var p = new PageModel()
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value
            };
            var list = Db.Queryable<User>()
                .OrderBy(it => it.Id, OrderByType.Desc)
                .ToPageList(p.PageIndex, p.PageSize);
            return list;
        }
        
        public bool UpdateUser(User user) {
            return this.Update(user);
        }
        
        
    }
}
