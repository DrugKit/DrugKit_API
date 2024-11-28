using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using DrugKitAPI.Core.Const;
using DrugKitAPI.Core.DTOs.Auth;
using DrugKitAPI.Core.Helpers;
using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace DrugKitAPI.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailingService _mailingService;
        private readonly IUnitOFWork _unitOfWork;
        private readonly IHostingEnvironment _webHostEnvironment; // Inject IWebHostEnvironment
        public AuthService(IUnitOFWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, IOptions<JWT> jwt, IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor, IMailingService mailingService, IHostingEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwt = jwt.Value;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _mailingService = mailingService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<AuthDTO> RegisterAsync(RegisterDTO model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthDTO { Message = "Email is already registered!" };

            var user = _mapper.Map<ApplicationUser>(model);
            user.UserName = model.Email;
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthDTO { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            var mobileUser = new MobileUser
            {
                ApplicationUserId = _userManager.FindByEmailAsync(model.Email).Result.Id
            };
            await _unitOfWork.MobileUser.AddAsync(mobileUser);
            await _unitOfWork.CompleteAsync();

            //
            var jwtSecurityToken = await CreateJwtToken(user);

            // Generate a 6-digit verification code
            var verificationCode = new Random().Next(100000, 999999);

            // Store the code (this could be stored in the database associated with the user or using a distributed cache)
            // For demonstration, assuming you save it in the database as a field called VerificationCode.
            user.VerificationCode = verificationCode.ToString();
            await _userManager.UpdateAsync(user);

            // Load email template
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot","HTML", "UserVerificationCode.html");
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Email template not found.", filePath);
            }

            var mailText = await File.ReadAllTextAsync(filePath);
            mailText = mailText.Replace("[name]", user.Name)
                               .Replace("[email]", user.Email)
                               .Replace("[code]", verificationCode.ToString()); // Here, replace the link placeholder with the code.

            // Send the 6-digit verification code to the parent's email
            await _mailingService.SendEmailAsync(user.Email, "Verification Code", mailText);

            return new AuthDTO
            {
                Email = user.Email,
                IsAuthenticated = true,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = new List<string> { Roles.User.ToString() },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }

        public async Task<AuthDTO> LoginAsync(LoginDTO model)
        {
            var authModel = new AuthDTO();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);
            authModel.Email = user.Email;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.IsAuthenticated = true;
            authModel.Roles = roles.ToList();
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return authModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            // Retrieve user claims and roles
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            // Create role claims
            var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var userIdLogged = "";
            userIdLogged = (await _unitOfWork.MobileUser.GetByAppUserIdAsync(user.Id.ToString())).Id.ToString();

            // Combine all claims into a single list
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // User ID
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),     // Username
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email),      // Email
                new Claim("LoggedId",userIdLogged.ToString())              // Id Of the LoggedIn User
            }
            .Union(userClaims)       // Include additional claims from user
            .Union(roleClaims);      // Include role claims

            // Create the signing key using the secret key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays), // Use UTC for expiration
                signingCredentials: signingCredentials,
                claims: claims
            );

            return jwtSecurityToken;
        }

    }
}