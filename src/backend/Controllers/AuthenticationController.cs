using Google.Apis.Oauth2.v2.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Authentication;
using Swallow.Models.DatabaseModels;
using Swallow.Services.Email;
using Swallow.Utils.Authentication;

namespace Swallow.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, EmailSender emailSender, ReCaptchaVerifier reCaptchaVerifier) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly EmailSender _emailSender = emailSender;
        private readonly ReCaptchaVerifier _reCaptchaVerifier = reCaptchaVerifier;
        private readonly AuthenticationProperties authenticationProperties = new()
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            AllowRefresh = true
        };

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginDto loginDto)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(loginDto.ReCaptchaToken))
            {
                return BadRequest("Invalid reCAPTCHA verification token.");
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, true, false);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid email or password.");
            }

            return Ok("Successfully logged in.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(registerDto.ReCaptchaToken))
            {
                return BadRequest("Invalid reCAPTCHA verification token.");
            }

            User user = new()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            if(await _userManager.FindByEmailAsync(registerDto.Email) is not null)
            {
                return Conflict("Email already exists.");
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid email or password.");
            }

            await _signInManager.SignInAsync(user, authenticationProperties, "Register");

            return Ok("Successfully registered user.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            
            return Ok("Successfully logged out.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(forgotPasswordDto.ReCaptchaToken))
            {
                return BadRequest("Invalid reCAPTCHA verification token.");
            }

            User? user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

            if (user is not null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _emailSender.ResetPasswordAsync(user.Email!, user.FullName, token);
            }

            return Ok("Successfully sent password reset email.");
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyTokenDto verifyTokenDto)
        {
            User? user = await _userManager.FindByEmailAsync(verifyTokenDto.Email);

            if (user is null)
            {
                return BadRequest("Invalid email.");
            }

            var result = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", verifyTokenDto.Token);

            if (!result)
            {
                return BadRequest("Invalid token.");
            }

            return Ok("Token is valid.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(resetPasswordDto.ReCaptchaToken))
            {
                return BadRequest("Invalid reCAPTCHA verification token.");
            }

            User? user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user is null)
            {
                return BadRequest("Invalid email.");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid password.");
            }

            return Ok("Successfully reset password.");
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto googleLoginDto)
        {
            Userinfo? userInfo = await GoogleTokenVerifier.VerifyTokenAndGetUserInfo(googleLoginDto.AccessToken);

            if (userInfo is null)
            {
                return BadRequest("Invalid Google access token.");
            }

            User? user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user is null)
            {
                user = new()
                {
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    FirstName = userInfo.GivenName,
                    LastName = userInfo.FamilyName,
                    ProfilePictureURL = userInfo.Picture
                };

                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to create user.");
                }
            }

            if (user.ProfilePictureURL != userInfo.Picture && user.CustomProfilePicture == false)
            {
                user.ProfilePictureURL = userInfo.Picture;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to update user.");
                }
            }

            await _signInManager.SignInAsync(user, authenticationProperties, "Google");

            return Ok("Successfully logged in.");
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            User? user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.CustomProfilePicture == true && user.ProfilePictureURL is not null)
            {
                string filename = Path.GetFileName(user.ProfilePictureURL);
                user.ProfilePictureURL = "http://localhost:5086/files/profile-picture?" + filename;
            }

            return Ok(UserResponse.Create(user));
        }
    }
}