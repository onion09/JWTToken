using JWTToken.Model.DBModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JWTToken.Util
{
    public interface IJwtUtils
    {
        string GenerateToken(User user, List<Permission> permissions);
        string ValidateToken(string token);
        List<string> ValidatePermissions(string token);
    }
    public class JWTTokenUtil : IJwtUtils
    {
        public string GenerateToken(User user, List<Permission> permissions)
        {
            var subject = new ClaimsIdentity();
            var claims = new List<Claim>();
            claims.Add(new Claim("id", user.UserID.ToString()));
            foreach(var permission in permissions)
            {
                claims.Add(new Claim($"permission{permission.PremissionId}", permission.PremissionName));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            //security key apply to token
            var key = Encoding.ASCII.GetBytes("superSecretKey@345");
            var tokenDescritopr = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
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
        public List<string> ValidatePermissions(string token)
        {
            if (token == null)
                return null;
            var key = Encoding.ASCII.GetBytes("superSecretKey@345");
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            //var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            //put permissions to Claims first
            var permissionClaims = ((JwtSecurityToken)validatedToken).Claims.Where(c => c.Type.StartsWith("permission")).ToList();
            var permissions = permissionClaims.Select(c => c.Value).ToList();

            //add user id to last of list. 
            var userID = ((JwtSecurityToken)validatedToken).Claims.First(x => x.Type == "id").Value;
            permissions.Add(userID);
            return permissions;
        }
    }
}
