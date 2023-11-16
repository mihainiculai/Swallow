using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.Models.DTOs.Authentication;
using Swallow.Services.Authentication;

namespace Swallow.Controllers
{
    /// <summary>
    /// Handles authentication-related actions such as login, registration, logout, and password management.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Logs in a user with the specified credentials.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <returns>An action result containing the login response.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _authService.LoginAsync(model);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var response = await _authService.RegisterAsync(model);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await _authService.LogoutAsync();
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var response = await _authService.ForgotPasswordAsync(model);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyTokenModel model)
        {
            var response = await _authService.VerifyResetTokenAsync(model);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var response = await _authService.ResetPasswordAsync(model);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginModel model)
        {
            var response = await _authService.GoogleLoginAsync(model);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.Message);
            }
            return Ok(response.Data);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var response = await _authService.GetCurrentUserAsync(User);
            if (!response.IsSuccess)
            {
                return StatusCode((int)response.StatusCode, response.Message);
            }
            return Ok(response.Data);
        }
    }
}