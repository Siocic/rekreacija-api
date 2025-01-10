using eRekreacija.Models.Models;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eRekreacijaAPI.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, int flag)
        {
            if (request == null)
                return BadRequest();

            var result = await _authService.RegisterUser(request, flag);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "User registered successfully" });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest();

            var result = await _authService.LoginAsync(request.Email, request.Password);
            if (string.IsNullOrEmpty(result))
                return Unauthorized("Invalid email or password");

            return Ok(new { Message = "Login successful", Token = result });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getAllUser")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _authService.GetAllUsersAsync();
            if (result == null)
                return NotFound("We dont have any users yet");
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _authService.GetAllRolesAsync();
            if (result == null)
                return NotFound("No roles found");
            return Ok(result);
        }
    }
}
