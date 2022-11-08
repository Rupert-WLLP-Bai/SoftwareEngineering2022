using System.Security.Claims;
using SimpleOJ.Model;

namespace SimpleOJ.Common
{
    public class UserToken
    {
        //public string? Jti { get; set; } //jwt id

        public string Id { get; set; } = "undefined";//user id

        public User.UserRole Role { get; set; } = User.UserRole.Student; //user role

        public string Ip { get; set; } = "undfined"; //

        public bool Remember { get; set; } = false; //是否记住该客户端


        public UserToken(string id, User.UserRole role)
        {
            Id = id;
            Role = role;
        }

    }
}