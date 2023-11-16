using Google.Apis.Oauth2.v2.Data;
using Microsoft.AspNetCore.Identity;
using Swallow.Models.DatabaseModels;
using Swallow.Models.DTOs.Authentication;
using Swallow.Services.Email;
using Swallow.Utils.Authentication;
using System.Net;
using System.Security.Claims;

namespace Swallow.Services.Authentication
{
    public interface IAuthService
    {
        Task<ServiceResponse<Response>> LoginAsync(LoginModel model);
        Task<ServiceResponse<Response>> RegisterAsync(RegisterModel model);
        Task<ServiceResponse<string>> LogoutAsync();
        Task<ServiceResponse<string>> ForgotPasswordAsync(ForgotPasswordModel model);
        Task<ServiceResponse<bool>> VerifyResetTokenAsync(VerifyTokenModel model);
        Task<ServiceResponse<bool>> ResetPasswordAsync(ResetPasswordModel model);
        Task<ServiceResponse<Response>> GoogleLoginAsync(GoogleLoginModel model);
        Task<ServiceResponse<Response>> GetCurrentUserAsync(ClaimsPrincipal user);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailSender _emailSender;
        private readonly ReCaptchaVerifier _reCaptchaVerifier;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, EmailSender emailSender, ReCaptchaVerifier reCaptchaVerifier)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _reCaptchaVerifier = reCaptchaVerifier;
        }

        public async Task<ServiceResponse<Response>> LoginAsync(LoginModel model)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(model.ReCaptchaToken))
            {
                return ServiceResponse<Response>.Failure("Invalid reCAPTCHA verification token.", HttpStatusCode.BadRequest);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

            if (!result.Succeeded)
            {
                return ServiceResponse<Response>.Failure("Invalid email or password.", HttpStatusCode.BadRequest);
            }

            User? user = await _userManager.FindByEmailAsync(model.Email);

            return ServiceResponse<Response>.Success(UserResponse.Create(user!));
        }

        public async Task<ServiceResponse<Response>> RegisterAsync(RegisterModel model)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(model.ReCaptchaToken))
            {
                return ServiceResponse<Response>.Failure("Invalid reCAPTCHA verification token.", HttpStatusCode.BadRequest);
            }

            User user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            if (_userManager.Users.Any(u => u.Email == user.Email))
            {
                return ServiceResponse<Response>.Failure("Email already exists.", HttpStatusCode.Conflict);
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return ServiceResponse<Response>.Failure("Failed to create user.", HttpStatusCode.InternalServerError);
            }

            await _signInManager.SignInAsync(user, true);

            return ServiceResponse<Response>.Success(UserResponse.Create(user));
        }

        public async Task<ServiceResponse<string>> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return ServiceResponse<string>.Success("Successfully logged out.");
        }

        public async Task<ServiceResponse<string>> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(model.ReCaptchaToken))
            {
                return ServiceResponse<string>.Failure("Invalid reCAPTCHA verification token.", HttpStatusCode.BadRequest);
            }

            User? user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && user.Email != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _emailSender.ResetPasswordAsync(user.Email, user.FullName, token);
            }

            return ServiceResponse<string>.Success("Successfully sent password reset email.");
        }

        public async Task<ServiceResponse<bool>> VerifyResetTokenAsync(VerifyTokenModel model)
        {
            User? user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return ServiceResponse<bool>.Failure("Invalid email.", HttpStatusCode.BadRequest);
            }

            var result = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", model.Token);

            if (!result)
            {
                return ServiceResponse<bool>.Failure("Invalid token.", HttpStatusCode.BadRequest);
            }

            return ServiceResponse<bool>.Success(true);
        }

        public async Task<ServiceResponse<bool>> ResetPasswordAsync(ResetPasswordModel model)
        {
            if (!await _reCaptchaVerifier.VerifyAsync(model.ReCaptchaToken))
            {
                return ServiceResponse<bool>.Failure("Invalid reCAPTCHA verification token.", HttpStatusCode.BadRequest);
            }

            User? user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return ServiceResponse<bool>.Failure("Invalid email.", HttpStatusCode.BadRequest);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
            {
                return ServiceResponse<bool>.Failure("Failed to reset password.", HttpStatusCode.InternalServerError);
            }

            return ServiceResponse<bool>.Success(true);
        }

        public async Task<ServiceResponse<Response>> GoogleLoginAsync(GoogleLoginModel model)
        {
            Userinfo? payload = await GoogleTokenVerifier.VerifyTokenAndGetUserInfo(model.AccessToken);

            if (payload == null)
            {
                return ServiceResponse<Response>.Failure("Invalid Google access token.", HttpStatusCode.BadRequest);
            }

            User? user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName
                };

                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    return ServiceResponse<Response>.Failure("Failed to create user.", HttpStatusCode.InternalServerError);
                }
            }

            await _signInManager.SignInAsync(user, true);

            return ServiceResponse<Response>.Success(UserResponse.Create(user));
        }

        public async Task<ServiceResponse<Response>> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            User? currentUser = await _userManager.GetUserAsync(user);

            if (currentUser == null)
            {
                return ServiceResponse<Response>.Failure("User not found.", HttpStatusCode.NotFound);
            }

            return ServiceResponse<Response>.Success(UserResponse.Create(currentUser));
        }
    }
}
