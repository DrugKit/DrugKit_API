using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DrugKitAPI.Core.Models;
using System;
using DrugKitAPI.EF.Data;
using Microsoft.AspNetCore.Identity;


namespace DrugKitAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<MobileUser>>> GetMobileUsers()
        {

            var roleName = "User";


            var userRoleId = await _context.Roles
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();



            var mobileUsers = await _context.MobileUsers
                .Include(mu => mu.ApplicationUser)  
                .Include(mu => mu.Reports)         
                .Where(mu => mu.ApplicationUser != null &&
                             _context.UserRoles.Any(ur => ur.UserId == mu.ApplicationUser.Id && ur.RoleId == userRoleId)) 
                .Select(mu => new
                {
                    UserId = mu.ApplicationUserId,
                    UserName = mu.ApplicationUser.Name,
                    UserEmail = mu.ApplicationUser.Email,
                    ReportsCount = mu.Reports.Count
                })
                .ToListAsync();

            return Ok(mobileUsers);
        }
    }
}




