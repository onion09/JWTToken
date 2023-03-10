using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JWTToken.Model.DBModel
{
    [Table("UserPermission")]

    public class UserPermission
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int PermissionId { get; set; }
        [JsonIgnore]

        public Permission Permission { get; set; }
    }
}
