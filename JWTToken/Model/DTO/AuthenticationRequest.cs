using System.ComponentModel.DataAnnotations;

namespace JWTToken.Model.DTO
{
    public class AuthenticationRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
