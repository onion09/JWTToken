using JWTToken.Util;
using JWTToken.Model.DBModel;

namespace JWTToken.Services
{
    public class UseerServices
    {
        public interface IUserService
        {
            string Authenticate(string username, string password);
        }

        public class UserService : IUserService
        {
            List<User> _users = new List<User> {
            new User{ UserID = 1, UserName = "test", Password = "test"}
        };

            private readonly IJwtUtils _jwtUtils;
            public UserService(IJwtUtils jwtUtils)
            {
                _jwtUtils = jwtUtils;
            }
            public string Authenticate(string username, string password)
            {
                var user = _users.SingleOrDefault(u => u.UserName == username && u.Password == password);
                if (user == null) return null;

                var token = _jwtUtils.GenerateToken(user);
                return token;

            }
        }
    }
}
