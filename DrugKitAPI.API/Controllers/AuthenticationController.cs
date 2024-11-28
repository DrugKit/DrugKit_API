using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using DrugKitAPI.Core.DTOs.Auth;
using DrugKitAPI.Core.Models;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DrugKitAPI.Core.Interfaces;
using AutoMapper;
using DrugKitAPI.Core.Services;
using DrugKitAPI.Core.Const;


namespace DrugKitAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOFWork _unitOfWork;
        private readonly IMailingService _mailingService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _webHostEnvironment; // Inject IWebHostEnvironment

        public AuthenticationController(IHostingEnvironment webHostEnvironment, IAuthService authService, UserManager<ApplicationUser> userManager, IUnitOFWork unitOfWork, IMapper mapper, IMailingService mailingService)
        {
            _authService = authService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mailingService = mailingService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!EmailValidatorService.IsValidEmailProvider(model.Email))
                return BadRequest("Invalid Email");

            try
            {
                var result = await _authService.RegisterAsync(model);
                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);
                return Ok(new { token = result.Token, expiresOn = result.ExpiresOn });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
 
        [HttpPost("login")]// Login for User or Admin 
        public async Task<IActionResult> LoginAsync(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.LoginAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (!user.EmailConfirmed)
                return BadRequest("Email not confirmed");

            if (await _userManager.IsInRoleAsync(user, Roles.User.ToString()))
            {
                var mobileUser = await _unitOfWork.MobileUser.GetByAppUserIdAsync(user.Id);
                return Ok(new
                {
                    token = result.Token,
                    expiresOn = result.ExpiresOn,
                    UserId = user.Id,
                    user.Email,
                    user.Name,
                    Role = Roles.User.ToString(),
                    MobileUserId = mobileUser.Id
                });
            }
            else if (await _userManager.IsInRoleAsync(user, Roles.Admin.ToString()))
            {
                var admin = await _unitOfWork.Admin.GetByAppUserIdAsync(user.Id);
                return Ok(new
                {
                    token = result.Token,
                    expiresOn = result.ExpiresOn,
                    UserId = user.Id,
                    user.Email,
                    user.Name,
                    Role = Roles.Admin.ToString(),
                    AdminId = admin.Id
                });
            }
            else
            {
                return Ok(new
                {
                    token = result.Token,
                    expiresOn = result.ExpiresOn,
                    UserId = user.Id,
                    user.Email,
                    user.Name,
                    Role = Roles.SuperAdmin.ToString()
                });

            }

        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await _unitOfWork.MobileUser.GetByIdAsync(model.Id) != null)
            {
                var mobileUser = await _unitOfWork.MobileUser.GetByIdAsync(model.Id);
                var user = await _userManager.FindByIdAsync(mobileUser.ApplicationUserId);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                // Check if the current password matches
                var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!isCurrentPasswordValid)
                {
                    return BadRequest("Current Password is incorrect!");
                }

                // Check if the new password is the same as the old one
                if (model.CurrentPassword == model.NewPassword)
                {
                    return BadRequest("Current password is similar to new password");
                }
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

                if (!result.Succeeded)
                {
                    // Return errors if password reset failed
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest($"Failed to update password: {errors}");
                }
                return Ok("Password updated successfully");
            }
            else
                return BadRequest("Invalid id");
        }
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("لم يتم العثور علي الإيميل");

            // Generate the reset code
            string resetCode = _mailingService.GenerateCode();

            // Set the reset code and expiration time
            user.ResetPasswordCode = resetCode;
            user.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(30); // Expiry time of 30 minutes

            // Save the reset code and expiration time in the database
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return StatusCode(500, "An error occurred while updating the user record.");

            // Load the email template
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot","HTML", "ResetPassword.html");
            string emailBody = await System.IO.File.ReadAllTextAsync(filePath);

            // Customize email body with reset code if needed
            emailBody = emailBody.Replace("{ResetCode}", resetCode);

            // Send the reset code via email
            await _mailingService.SendEmailAsync(
                model.Email,
                "Code For Reset Password",
                emailBody // Using the modified template with the reset code
            );

            return Ok("Code sent to your mail successfully");
        }
        [HttpPost("check-reset-code")]
        public async Task<IActionResult> CheckResetCode(CheckResetCodeDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("No user found with this email");

            // Check if the reset code matches and has not expired
            if (user.ResetPasswordCode != model.ResetCode || user.ResetCodeExpiry < DateTime.UtcNow)
                return BadRequest("The reset code is invalid or has expired.");

            // Code is valid
            return Ok("Reset code is valid.");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("No user found with this email");

            if (user.ResetPasswordCode != model.ResetCode || user.ResetCodeExpiry < DateTime.UtcNow)
                return BadRequest("The reset code is invalid or has expired.");
            // Reset the password
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Clear the reset code after successful password reset
            user.ResetPasswordCode = null;
            user.ResetCodeExpiry = null;
            await _userManager.UpdateAsync(user);

            return Ok("Password changed successfully");
        }
        [HttpPost("verify-registration-code")]
        public async Task<IActionResult> VerifyParentCode(VerifyParentCodeDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("No user found with this email");

            // Verify the code
            if (user.VerificationCode != model.VerificationCode)
                return BadRequest("The verification code is invalid.");

            // Code matches, mark IsConfirmed as true
            user.EmailConfirmed = true;

            // Clear the verification code to prevent reuse
            user.VerificationCode = null;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Failed to update confirmation status.");

            var loginModelDto = new LoginDTO
            {
                Email = model.Email,
                Password = model.Password
            };
            var result2 = await _authService.LoginAsync(loginModelDto);

            if (!result2.IsAuthenticated)
                return BadRequest(result2.Message);

            var mobileUser = await _unitOfWork.MobileUser.GetByAppUserIdAsync(user.Id);
            return Ok(new
            {
                token = result2.Token,
                expiresOn = result2.ExpiresOn,
                UserId = user.Id,
                user.Email,
                user.Name,
                Role = result2.Roles,
                MobileUserId = mobileUser.Id
            });
        }
        [HttpPost("resend-verification-code")]
        public async Task<IActionResult> ResendVerificationCode(ResendVerificationCodeDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("No user found with this email");

            // Check if the user is already confirmed
            if (user.EmailConfirmed)
                return BadRequest("The account is already confirmed.");

            // Generate a new 6-digit verification code
            var newVerificationCode = new Random().Next(100000, 999999).ToString();

            // Store the new verification code
            user.VerificationCode = newVerificationCode;

            // Update the user in the database
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return StatusCode(500, "An error occurred while updating the user record.");

            // Load email template
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "HTML", "UserVerificationCode.html");

            var mailText = await System.IO.File.ReadAllTextAsync(filePath);
            mailText = mailText.Replace("[name]", user.Name)
                               .Replace("[email]", user.Email)
                               .Replace("[code]", newVerificationCode); // Replace link with the new verification code

            // Send the email with the new verification code
            await _mailingService.SendEmailAsync(user.Email, "Resend Verification Code", mailText);

            return Ok("A new verification code has been sent to your email.");
        }

    }
}