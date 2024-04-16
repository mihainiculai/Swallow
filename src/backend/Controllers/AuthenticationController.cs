using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swallow.Configs;
using Swallow.DTOs.Authentication;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using Swallow.Services.Email;
using Swallow.Utils.Authentication;

namespace Swallow.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, IUserRepository userRepository, IGoogleTokenVerifier googleTokenVerifier, AuthenticationPropertiesConfig authenticationPropertiesConfig, ISubscriptionRepository subscriptionRepository, IEmailSender emailSender, IReCaptchaVerifier reCaptchaVerifier, IMapper mapper) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LoginDto loginDto)
        {
            await reCaptchaVerifier.VerifyAsync(loginDto.ReCaptchaToken);

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
            await reCaptchaVerifier.VerifyAsync(registerDto.ReCaptchaToken);

            var user = mapper.Map<User>(registerDto);

            if (await userManager.FindByEmailAsync(registerDto.Email) is not null)
            {
                return Conflict("Email already exists.");
            }

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid email or password.");
            }

            await subscriptionRepository.CreateUserSubscription(user);

            await signInManager.SignInAsync(user, authenticationPropertiesConfig.Properties, "Register");

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
            await reCaptchaVerifier.VerifyAsync(forgotPasswordDto.ReCaptchaToken);

            var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);

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
            var user = await userManager.FindByEmailAsync(verifyTokenDto.Email);

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
            await reCaptchaVerifier.VerifyAsync(resetPasswordDto.ReCaptchaToken);

            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);

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
            var userInfo = await googleTokenVerifier.GetUserInfo(googleLoginDto.AccessToken);

            var user = await userManager.FindByEmailAsync(userInfo.Email);

            if (user is null)
            {
                user = mapper.Map<User>(userInfo);

                var result = await userManager.CreateAsync(user);

                if (userInfo.EmailVerified)
                {
                    await userManager.ConfirmEmailAsync(user, await userManager.GenerateEmailConfirmationTokenAsync(user));
                }

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to create user.");
                }

                await subscriptionRepository.CreateUserSubscription(user);
            }
            else
            {
                user = mapper.Map(userInfo, user);

                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest("Failed to update user.");
                }
            }

            await signInManager.SignInAsync(user, authenticationPropertiesConfig.Properties, "Google");

            return Ok("Successfully logged in.");
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await userManager.GetUserAsync(User);

            if (user is null)
            {
                return BadRequest("Invalid user.");
            }

            var userPlan = await userRepository.GetCurrentSubscription(User);

            var result = mapper.Map<UserDto>(user);
            result.PlanId = userPlan.PlanId;

            return Ok(result);
        }
    }
}