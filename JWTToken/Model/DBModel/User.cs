using System.ComponentModel.DataAnnotations.Schema;

namespace JWTToken.Model.DBModel
{
    [Table("UserInfo")]

    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
