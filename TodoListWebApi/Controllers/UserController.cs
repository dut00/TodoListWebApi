using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoListWebApi.DTOs;
using TodoListWebApi.Entities;
using TodoListWebApi.Services;

namespace TodoListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<string> Me()
        {
            ClaimsPrincipal userClaims = _userService.GetMyClaims();
            string name = userClaims.FindFirstValue(ClaimTypes.Name);
            string role = userClaims.FindFirstValue(ClaimTypes.Role);

            return Ok($"Your name is: {name}\nRole: {role}");
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO request)
        {
            if (!_userService.CreateUser(request, out User? user))
                return BadRequest(new { message = "Provided username is already taken. Try a different one." });

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            User? user = _userService.GetUser(request.Username);

            if (user == null)
                return BadRequest("User not found.");

            if (!_userService.VerifyPasswordHash(request.Password, user.PasswordSalt, user.PasswordHash))
                return BadRequest("Wrong password.");

            string token = _userService.CreateToken(user);

            return Ok(token);
        }
    }
}
