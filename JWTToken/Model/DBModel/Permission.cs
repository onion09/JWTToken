using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTToken.Model.DBModel

{
    [Table("Permission")]

    public class Permission
    {
        [Key]
        public int PremissionId { get; set; }
        public string PremissionName { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
