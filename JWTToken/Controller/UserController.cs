using Azure.Core;
using JWTToken.Exceptions;
using JWTToken.Filter;
using JWTToken.Model.DBModel;
using JWTToken.Model.DTO;
using JWTToken.Model.ViewModel;
using JWTToken.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static JWTToken.Services.UseerServices;
using AuthorizeAttribute = JWTToken.Util.Authorize1Attribute;

namespace JWTToken.Controller
{
    [ApiController]
    [Route("/user")]
    //[AnotherExceptionFilter]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserService _user;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, UserService user, ILogger<UserController> logger)
        {
            _userService = userService;
            _user = user;
            _logger = logger;
        }

        [Authorize1]
        [HttpGet("test")]
        public string Test()
        {
            return "test";
        }

        //[AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthenticationRequest user)
        {
            _logger.LogInformation($"Login get called at {DateTime.Now}");

            var token = _userService.Authenticate(user.Username, user.Password);
            if (token == null)
                return BadRequest(new { message = "username or password not match" });
            return Ok(new { message = "Successfully Authenticaed", accessToken = token} );

        }

        [HttpPost("/user")]
        public ActionResult Registration([FromBody] UserRegistration userInfoWhole)
        {
            _logger.LogInformation($"Registration get called at {DateTime.Now}");

            try { _user.Registration(userInfoWhole); }
            catch (UserNotFoundException ex) { }
            return Ok();
        }
        
        [HttpDelete("/user")]
        public ActionResult<int> DeleteUser([FromQuery] int userId)
        {
            _logger.LogInformation($"Delete User {userId} get called at {DateTime.Now}");

            _user.DeleteUser(userId);
            return Ok(userId);
        }

        [UpdateAuthoriaztion]

        [HttpPatch("/user/{userId}/status")]
        public ActionResult<User> UpdateActivation([FromQuery] bool activate, int userId)
        {
            _logger.LogInformation($"UpdateActivation {userId} to{activate} at {DateTime.Now}");

            var user =   _user.UserActivation(userId, activate);
            if (user == null)
            {
                _logger.LogError("Error Log for testing");

                throw new UserNotFoundException($"UserId {userId} not found.");
            }
            return Ok(user);
        }
        [HttpGet("/user")]
        public ActionResult<List<UserDisplay>> ListAllUser()
        {
            _logger.LogInformation($"ListAllUser get called at {DateTime.Now}");

            var list = _user.GetAllUser();
            if(list == null)
            {
                throw new UserNotFoundException("User Not found, check your database");
            }
            return Ok(list);
        }
        [HttpGet("/user/info/{userId}")]

        public ActionResult<GetUser> GetUserById(int userId)
        {
            _logger.LogInformation($"GetUserById id: {userId} get called at {DateTime.Now}");

            GetUser userInfo = _user.GetUserById(userId);
            if (userInfo == null) 
            { 
                throw new UserNotFoundException($"UserId {userId} not found.");
            }
            return Ok(userInfo);
        }

    }
}
