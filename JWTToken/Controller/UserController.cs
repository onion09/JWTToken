using JWTToken.Model.DTO;
using JWTToken.Util;
using Microsoft.AspNetCore.Mvc;
using static JWTToken.Services.UseerServices;

namespace JWTToken.Controller
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("test")]
        public string Test()
        {
            return "test";
        }

        /*
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthenticationRequest user)
        {
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }
            if (user.Username == "test" && user.Password == "test")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44370",
                    audience: "https://localhost:44370",
                    claims: new List<Claim>() { new Claim("id", "1") },
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new AuthenticationResponse { Token = tokenString });
            }
            return Unauthorized();
        }
        */


        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthenticationRequest user)
        {
            var token = _userService.Authenticate(user.Username, user.Password);
            if (token == null)
                return BadRequest(new { message = "username or password not match" });
            return Ok(new AuthenticationResponse { Token = token });

        }

    }
}
