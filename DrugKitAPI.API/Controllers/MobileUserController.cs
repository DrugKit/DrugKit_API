using DrugKitAPI.Core.DTOs.MobileUser;
using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DrugKitAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileUserController : ControllerBase
    {
        private readonly IUnitOFWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager; 
        public MobileUserController(IUnitOFWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMobileUsers()
        {
            var mobileUsers = await _unitOfWork.MobileUser.GetAllAsync();
            var result = new List<MobileUserDTO>();
            foreach(var  mobileUser in mobileUsers)
            {
                var user = await _userManager.FindByIdAsync(mobileUser.ApplicationUserId);
                var userDto = new MobileUserDTO
                {
                    Id = mobileUser.Id,
                    Name = user.Name,
                    Email = user.Email,
                    ReportsCount = _unitOfWork.Report.FindAllAsync(r => r.MobileUserId == mobileUser.Id).Result.Count()
                };
                result.Add(userDto);
            }
            return Ok(result);
        }
    }
}
