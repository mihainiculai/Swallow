using AutoMapper;
using Google.Apis.Oauth2.v2.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    public class AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, EmailSender emailSender, ReCaptchaVerifier reCaptchaVerifier, IMapper mapper) : ControllerBase
    {
        private readonly AuthenticationProperties authenticationProperties = new()
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(400),
            AllowRefresh = true
        };

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginDto loginDto)
        {
            if (!await reCaptchaVerifier.VerifyAsync(loginDto.ReCaptchaToken))
            {
                return BadRequest("Invalid reCAPTCHA verification token.");
            }

            var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, true, false);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid email or password.");
            }

            return Ok("Successfully logged in.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!await reCaptchaVerifier.VerifyAsync(registerDto.ReCaptchaToken))
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

            if(await userManager.FindByEmailAsync(registerDto.Email) is not null)
            {
                return Conflict("Email already exists.");
            }

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid email or password.");
            }

            await signInManager.SignInAsync(user, authenticationProperties, "Register");

            return Ok("Successfully registered user.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            
            return Ok("Successfully logged out.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!await reCaptchaVerifier.VerifyAsync(forgotPasswordDto.ReCaptchaToken))
            {
                return BadRequest("Invalid reCAPTCHA verification token.");
            }

            User? user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);

            if (user is not null)
            {
                string token = await userManager.GeneratePasswordResetTokenAsync(user);

                await emailSender.ResetPasswordAsync(user.Email!, user.FullName, token);
            }

            return Ok("Successfully sent password reset email.");
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyTokenDto verifyTokenDto)
        {
            User? user = await userManager.FindByEmailAsync(verifyTokenDto.Email);

            if (user is null)
            {
                return BadRequest("Invalid email.");
            }

            var result = await userManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", verifyTokenDto.Token);

            if (!result)
            {
                return BadRequest("Invalid token.");
            }

            return Ok("Token is valid.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!await reCaptchaVerifier.VerifyAsync(resetPasswordDto.ReCaptchaToken))
            {
                return BadRequest("Invalid reCAPTCHA verification token.");
            }

            User? user = await userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user is null)
            {
                return BadRequest("Invalid email.");
            }

            var result = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

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

            User? user = await userManager.FindByEmailAsync(userInfo.Email);

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

                var result = await userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to create user.");
                }
            }

            if (user.ProfilePictureURL != userInfo.Picture && user.CustomProfilePicture == false)
            {
                user.ProfilePictureURL = userInfo.Picture;

                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to update user.");
                }
            }

            await signInManager.SignInAsync(user, authenticationProperties, "Google");

            return Ok("Successfully logged in.");
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            User? user = await userManager.GetUserAsync(User);

            if (user is null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.CustomProfilePicture == true && user.ProfilePictureURL is not null)
            {
                string filename = Path.GetFileName(user.ProfilePictureURL);
                user.ProfilePictureURL = "http://localhost:5086/files/profile-picture?" + filename;
            }

            return Ok(mapper.Map<UserDto>(user));
        }
    }
}