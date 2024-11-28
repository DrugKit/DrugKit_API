using DrugKitAPI.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DrugKitAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        private readonly IUnitOFWork _unitOFWork;
        public DrugController(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

    }
}
