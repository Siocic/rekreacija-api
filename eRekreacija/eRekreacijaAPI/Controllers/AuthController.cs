using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.enums;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eRekreacijaAPI.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;


        public AuthController(IAuthService authService, ILogger<AuthController> logger,UserManager<ApplicationUser>userManager)
        {
            _authService = authService;
            _logger = logger;
            _userManager = userManager;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest? request, int flag)
        {
            if (request == null)
                return BadRequest(new { Message = "Invalid request data" });

            var result = await _authService.RegisterUser(request, flag);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { Message = $"Registration failed. {errorMessages}" });
            }

            return Ok(new { Message = "User registered successfully" });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Invalid request data" });

            var result = await _authService.LoginAsync(request.Email, request.Password);
            if (string.IsNullOrEmpty(result) || result == "User not found")
                return BadRequest(new { Message = "Invalid email or password" });

            return Ok(new { Message = "Login successful", Token = result });
        }

        [Authorize(AuthenticationSchemes = "Bearer",Roles ="SuperAdmin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult>GetAll()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _authService.GetAllUsers(userId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getUserOfPravnoLice")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _authService.GetAllUserOfRolePravnoLice();
            if (result == null)
                return NotFound("We dont have any users yet");
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getUserOfFizickoLice")]
        public async Task<IActionResult> GetAllFizckoLiceUsers()
        {
            var result = await _authService.GetAllUserOfRoleFizikoLice();
            if (result == null)
                return NotFound("We dont have any users yet");
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getNotApprovedUser")]
        public async Task<IActionResult> GetAllPravnoLiceThatNotApprovedYet()
        {
            var result = await _authService.GetAllPravnoLiceThatNotApprovedYet();
            if (result == null)
                return NotFound("We dont have any users yet");
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getUser")]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var result = await _authService.GetUser(userId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("editUser")]
        public async Task<IActionResult> EditProfile([FromBody] ApplicationUserDTO model)
        {
            if (model == null)
                return BadRequest();

            var result = await _authService.EditProfile(model);

            return Ok();
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("change")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            if (model == null)
                return BadRequest(new { Message = "The form is invalid" });

            try
            {
                var result = await _authService.ChangePassword(model, userId);
                if (result == 0)
                    return BadRequest(new { Message = "User not found" });
                else if (result == -1)
                    return BadRequest(new { Message = "Incorrect current password" });
                else if (result == -2)
                    return BadRequest(new { Message = "Failed to change password" });

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, ex.InnerException?.Message, ex.Message);
                return BadRequest();
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("approve-registartion")]
        public async Task<IActionResult>ApproveRegistration(string userId)
        {
            if (userId == null)
                return BadRequest("The model is invalid");

            var result = await _authService.ApproveRegistration(userId);
             return Ok("The registartion is approve successfully");
        }
    }
}
