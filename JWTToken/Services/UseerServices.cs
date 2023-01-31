using JWTToken.Util;
using JWTToken.Model.DBModel;
using Microsoft.Identity.Client;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using JWTToken.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

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
            private readonly IConfiguration _configuration;
            private readonly AuthDBContext _dbContext;
            private readonly IJwtUtils _jwtUtils;

            private readonly string _checkIfMatchQuery = "SELECT COUNT(*) FROM userInfo WHERE UserName = @username AND Password = @password";

            public UserService(IJwtUtils jwtUtils, IConfiguration configuration, AuthDBContext authDB)
            {
                _jwtUtils = jwtUtils;
                _configuration = configuration;
                _dbContext = authDB;
            }
            public string Authenticate(string username, string password)
            {
                //var user = new User();
                //using (var conn = new SqlConnection(_configuration.GetConnectionString("MyConn1")))
                //{
                //    conn.Open();
                //    var cmd = new SqlCommand(_checkIfMatchQuery, conn);
                //    cmd.Parameters.AddWithValue("@username", username);
                //    cmd.Parameters.AddWithValue("@password", password);
                //    //varify user credential
                //    var count = Convert.ToInt32(cmd.ExecuteScalar());
                //    if (count > 0)
                //    {
                //        user.Password = password;
                //        user.UserName = username;
                //    }
                //    else return null;
                //}
                var user = _dbContext.Users.FirstOrDefault(x=>x.UserName == username && x.Password== password);
                if (user == null) return null;

                var token = _jwtUtils.GenerateToken(user);
                return token;

            }
            public void Registration(UserRegistration userRegistration)
            {
                var newuserDetail = new UserDetail { Email = userRegistration.email, FirstName=userRegistration.firstname, LastName=userRegistration.lastname};
                var newuser = new User {UserName = userRegistration.username, Password=userRegistration.password, UserDetail = newuserDetail};
                _dbContext.Users.Add(newuser);
                _dbContext.SaveChanges();
                 
            }
            private User FindUserById(int id)
            {
                var user = _dbContext.Users.Include(q=>q.UserPermissions).Include(q=>q.UserDetail).Where(q=>q.UserID == id).FirstOrDefault();
                return user;
            }
            public int DeleteUser(int userId)
            {
                var user = FindUserById(userId);
                if(user != null)
                {
                    foreach(var userPermission in user.UserPermissions.ToList())
                    {
                        _dbContext.UserPermissions.Remove(userPermission); 
                    }
                    _dbContext.Users.Remove(user);
                    _dbContext.SaveChanges();
                }
                return userId;
            }
            public User UserActivation(int userId, bool activate)
            {
                var user = FindUserById(userId);
                if(user != null)
                {
                    user.Staus = activate;
                    _dbContext.SaveChanges();
                }
                return user;
            }
            public List<UserDisplay> GetAllUser()
            {
                var list = _dbContext.Users.ToList();
                var userlist = new List<UserDisplay>();
                foreach(var item in list)
                {
                    userlist.Add(new UserDisplay { userId = item.UserID, UserName = item.UserName });
                }
                return userlist;
            }
            public GetUser GetUserById(int id)
            {
                var userdetail = new GetUser();
                var user = _dbContext.Users.Include(q => q.UserDetail).Where(q => q.UserID == id).FirstOrDefault();
                if(user != null)
                {
                    userdetail.UserId = id;
                    userdetail.Email=user.UserDetail.Email;
                    userdetail.FirstName= user.UserDetail.FirstName;
                    userdetail.LastName= user.UserDetail.LastName;
                    userdetail.username = user.UserName;
                }
                return userdetail;
            }
        }
    }
}
