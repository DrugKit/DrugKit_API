using DrugKitAPI.Core.DTOs.Donation;
using DrugKitAPI.Core.Interfaces;
using DrugKitAPI.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DrugKitAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IUnitOFWork _unitOfWork;

        public DonationController(IUnitOFWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateDonationPost")]
        public async Task<IActionResult> CreateDonationPost([FromForm] DonationDTO donation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (donation.DonationImages.Count == 0)
                return BadRequest("You should send at least one image");

            // Check if the mobile user exists
            var mobileUser = await _unitOfWork.MobileUser.GetByIdAsync(donation.MobileUserId);
            if (mobileUser == null)
                return NotFound($"No user with ID: {donation.MobileUserId}");

            // Create the Donation entity
            var donationEntity = new Donation
            {
                DrugName = donation.DrugName,
                ExpirationDate = donation.ExpirationDate,
                MobileUserId = donation.MobileUserId,
                Date = donation.Date,
                DonationImgs = new List<DonationImg>(),
                IdentityPath = string.Empty
            };

            // Add the Donation to the database
            await _unitOfWork.Donation.AddAsync(donationEntity);
            await _unitOfWork.CompleteAsync();

            // Handle the IdentityImage
            if (donation.IdentityImage != null)
            {
                var identityFolderPath = Path.Combine("wwwroot", "Images", "IdentityImages");
                if (!Directory.Exists(identityFolderPath))
                    Directory.CreateDirectory(identityFolderPath);

                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var identityFileName = $"{Path.GetFileNameWithoutExtension(donation.IdentityImage.FileName)}_{timestamp}{Path.GetExtension(donation.IdentityImage.FileName)}";
                var identityFilePath = Path.Combine(identityFolderPath, identityFileName);

                using (var stream = new FileStream(identityFilePath, FileMode.Create))
                {
                    await donation.IdentityImage.CopyToAsync(stream);
                }

                donationEntity.IdentityPath = identityFilePath;
            }

            // Handle DonationImages
            if (donation.DonationImages != null && donation.DonationImages.Any())
            {
                var donationFolderPath = Path.Combine("wwwroot", "Images", "DonationImages");
                if (!Directory.Exists(donationFolderPath))
                    Directory.CreateDirectory(donationFolderPath);

                foreach (var donationImage in donation.DonationImages)
                {
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var donationFileName = $"{Path.GetFileNameWithoutExtension(donationImage.FileName)}_{timestamp}{Path.GetExtension(donationImage.FileName)}";
                    var donationFilePath = Path.Combine(donationFolderPath, donationFileName);

                    using (var stream = new FileStream(donationFilePath, FileMode.Create))
                    {
                        await donationImage.CopyToAsync(stream);
                    }

                    donationEntity.DonationImgs.Add(new DonationImg
                    {
                        DonationId = donationEntity.Id,
                        ImagPath = donationFilePath
                    });
                }
            }

            await _unitOfWork.CompleteAsync();

            return Ok(new { Message = "Donation uploaded successfully" });
        }

        [HttpPost("CreateDonationRequest")]
        public async Task<IActionResult> CreateDonationRequest([FromForm] UserRequestDonationDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _unitOfWork.UserRequestedDonation.FindTWithExpression<UserRequestedDonation>(u => u.MobileUserId == request.MobileUserId
            && u.DonationId == request.DonationId) != null)
                return BadRequest($"this donation is already requested by userId: {request.MobileUserId}");

                var mobileUser = await _unitOfWork.MobileUser.GetByIdAsync(request.MobileUserId);
            if (mobileUser == null)
                return NotFound($"No user found with ID: {request.MobileUserId}");

            var donation = await _unitOfWork.Donation.GetByIdAsync(request.DonationId);
            if (donation == null)
                return NotFound($"No donation found with ID: {request.DonationId}");

            string identityImagePath = string.Empty;
            string medicalReportPath = string.Empty;

            if (request.IdentityImage != null)
            {
                var identityFolderPath = Path.Combine("wwwroot", "Images", "IdentityImages");
                if (!Directory.Exists(identityFolderPath))
                    Directory.CreateDirectory(identityFolderPath);

                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var identityFileName = $"{Path.GetFileNameWithoutExtension(request.IdentityImage.FileName)}_{timestamp}{Path.GetExtension(request.IdentityImage.FileName)}";
                identityImagePath = Path.Combine(identityFolderPath, identityFileName);

                using (var stream = new FileStream(identityImagePath, FileMode.Create))
                {
                    await request.IdentityImage.CopyToAsync(stream);
                }
            }

            if (request.MedicalReportImage != null)
            {
                var reportFolderPath = Path.Combine("wwwroot", "Images", "MedicalReports");
                if (!Directory.Exists(reportFolderPath))
                    Directory.CreateDirectory(reportFolderPath);

                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var reportFileName = $"{Path.GetFileNameWithoutExtension(request.MedicalReportImage.FileName)}_{timestamp}{Path.GetExtension(request.MedicalReportImage.FileName)}";
                medicalReportPath = Path.Combine(reportFolderPath, reportFileName);

                using (var stream = new FileStream(medicalReportPath, FileMode.Create))
                {
                    await request.MedicalReportImage.CopyToAsync(stream);
                }
            }

            var userRequestDonation = new UserRequestedDonation
            {
                MobileUserId = request.MobileUserId,
                DonationId = request.DonationId,
                Message = request.Message,
                PhoneNumber = request.PhoneNumber,
                Date = request.Date,
                IdentityPath = identityImagePath,
                MedicalReportPath = medicalReportPath
            };

            await _unitOfWork.UserRequestedDonation.AddAsync(userRequestDonation);
            await _unitOfWork.CompleteAsync();

            return Ok(new { Message = "Donation request created successfully" });
        }

        [HttpGet("GetUserRequest")]
        public async Task<IActionResult> GetUserRequestById(int userId, int donationId)
        {
            var userRequest = await _unitOfWork.UserRequestedDonation.FindTWithExpression<UserRequestedDonation>(
                u=>u.MobileUserId==userId&&
                u.DonationId==donationId
                );

            if (userRequest == null)
                return NotFound(new { Message = $"No user request found with UserID: {userId} and DonationID: {donationId}" });

            var userRequestDTO = new
            {
                MobileUserId = userRequest.MobileUserId,
                DonationId = userRequest.DonationId,
                Message = userRequest.Message,
                PhoneNumber = userRequest.PhoneNumber,
                Date = userRequest.Date,
                IdentityPath = userRequest.IdentityPath,
                MedicalReportPath = userRequest.MedicalReportPath
            };

            return Ok(userRequestDTO);
        }

        [HttpDelete("DeleteUserRequest")]
        public async Task<IActionResult> DeleteUserRequest(int userId, int donationId)
        {
            var userRequest = await _unitOfWork.UserRequestedDonation.FindTWithExpression<UserRequestedDonation>(
                u => u.MobileUserId == userId && u.DonationId == donationId);

            if (userRequest == null)
                return NotFound(new { Message = $"No user request found with UserID: {userId} and DonationID: {donationId}" });

            if (!string.IsNullOrEmpty(userRequest.IdentityPath) && System.IO.File.Exists(userRequest.IdentityPath))
            {
                System.IO.File.Delete(userRequest.IdentityPath); // Delete Identity Image
            }

            if (!string.IsNullOrEmpty(userRequest.MedicalReportPath) && System.IO.File.Exists(userRequest.MedicalReportPath))
            {
                System.IO.File.Delete(userRequest.MedicalReportPath); // Delete Medical Report Image
            }

            await _unitOfWork.UserRequestedDonation.DeleteAsync(userRequest);

            await _unitOfWork.CompleteAsync();

            return Ok(new { Message = "User request and related images deleted successfully" });
        }
        //[HttpGet("GetDonationPostersByUserId")]
        //public async Task<IActionResult> GetDonationPostersByUserId(int userId)
        //{
        //    var donations = await _unitOfWork.Donation.FindAllAsync(d => d.MobileUserId == userId);
            
        //    foreach(var donation in donations)
        //    {

        //    }
            

        //    return Ok(donationDTO);
        //}



    }
}
