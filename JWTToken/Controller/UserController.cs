using Azure.Core;
using JWTToken.Model.DBModel;
using JWTToken.Model.DTO;
using JWTToken.Model.ViewModel;
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
        private readonly UserService _user;
        public UserController(IUserService userService, UserService user)
        {
            _userService = userService;
            _user = user;
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
            return Ok(new { message = "Successfully Authenticaed", accessToken = token} );

        }
        [HttpPost("/user")]
        public ActionResult Registration([FromBody] UserRegistration userInfoWhole)
        {
            _user.Registration(userInfoWhole);
            return Ok();
        }
        [HttpDelete("/user")]
        public ActionResult<int> DeleteUser([FromQuery] int userId)
        {
            _user.DeleteUser(userId);
            return Ok(userId);
        }
        [HttpPatch("/user/{userId}/status")]
        public ActionResult<User> UpdateActivation([FromQuery] bool activate, int userId)
        {
            var user =   _user.UserActivation(userId, activate);
            return Ok(user);
        }
        [HttpGet("/user")]
        public ActionResult<List<UserDisplay>> ListAllUser()
        {
            var list = _user.GetAllUser();
            return Ok(list);
        }
        [HttpGet("/user/info/{userId}")]

        public ActionResult<GetUser> GetUserById(int userId)
        {
            GetUser userInfo = _user.GetUserById(userId);
            if (userInfo == null) { return NotFound(userId); }
            return Ok(userInfo);
        }

    }
}
