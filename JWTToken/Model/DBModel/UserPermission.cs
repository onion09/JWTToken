using System.ComponentModel.DataAnnotations.Schema;

namespace JWTToken.Model.DBModel
{
    [Table("UserPermission")]

    public class UserPermission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PermissionId { get; set; }   
        public Permission Permission { get; set; }
    }
}
