﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swallow.Models;
using Swallow.Services.Email;
using Swallow.Utils;
using System.Net;

namespace Swallow.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailSender _emailSender;
        private readonly ReCaptchaVerifier _reCaptchaVerifier;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, EmailSender emailSender, ReCaptchaVerifier reCaptchaVerifier)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _reCaptchaVerifier = reCaptchaVerifier;
        }

        public class LoginModel
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required string ReCaptchaToken { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(model.ReCaptchaToken))
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            User? user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return NotFound();
            }

            var response = new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.ProfilePictureURL
            };

            return Ok(response);
        }

        public class RegisterModel
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
            public required string FirstName { get; set; }
            public required string LastName { get; set; }
            public required string ReCaptchaToken { get; set; }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(model.ReCaptchaToken))
            {
                return BadRequest();
            }

            User user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return Conflict();
            }

            await _signInManager.SignInAsync(user, true);

            var response = new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                user.ProfilePictureURL
            };

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        public class ForgotPasswordModel
        {
            public required string Email { get; set; }
            public required string ReCaptchaToken { get; set; }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(model.ReCaptchaToken))
            {
                return BadRequest();
            }

            User? user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || user.Email == null)
            {
                return Ok();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _emailSender.ResetPasswordAsync(user.Email, user.FullName, token);

            return Ok();
        }

        public class VerifyTokenModel
        {
            public required string Email { get; set; }
            public required string Token { get; set; }
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyTokenModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var isValid = await _userManager.VerifyUserTokenAsync(
                user,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                UserManager<User>.ResetPasswordTokenPurpose,
                model.Token
            );

            if (!isValid)
            {
                return NotFound();
            }

            return Ok();
        }

        public class ResetPasswordModel
        {
            public required string Email { get; set; }
            public required string Token { get; set; }
            public required string Password { get; set; }
            public required string ReCaptchaToken { get; set; }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(model.ReCaptchaToken))
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!resetPassResult.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        public class GoogleLoginModel
        {
            public required string AccessToken { get; set; }
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginModel model)
        {
            var credential = GoogleCredential.FromAccessToken(model.AccessToken);

            var oauth2Service = new Oauth2Service(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            var userInfoRequest = oauth2Service.Userinfo.Get();
            var userInfo = await userInfoRequest.ExecuteAsync();

            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user == null)
            {
                user = new User
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
                    return BadRequest(result.Errors);
                }
            }

            await _signInManager.SignInAsync(user, true);

            var response = new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                user.ProfilePictureURL
            };

            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            User? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var response = new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                user.ProfilePictureURL
            };

            return Ok(response);
        }
    }
}
