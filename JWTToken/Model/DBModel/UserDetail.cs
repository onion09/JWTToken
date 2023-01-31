using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JWTToken.Model.DBModel
{
    [Table("UserDetail")]

    public class UserDetail
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
