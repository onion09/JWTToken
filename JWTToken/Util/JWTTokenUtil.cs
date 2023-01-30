using JWTToken.Model.DBModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTToken.Util
{
    public interface IJwtUtils
    {
        string GenerateToken(User user);
        string ValidateToken(string token);
    }
    public class JWTTokenUtil : IJwtUtils
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //security key apply to token
            var key = Encoding.ASCII.GetBytes("superSecretKey@345");
            var tokenDescritopr = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserID.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescritopr);
            return tokenHandler.WriteToken(token);
        }

        public string ValidateToken(string token)
        {
            if (token == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("superSecretKey@345");
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var userID = ((JwtSecurityToken)validatedToken).Claims.First(x => x.Type == "id").Value;
            return userID;
        }
    }
}
