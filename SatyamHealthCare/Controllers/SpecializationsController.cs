using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecialization _specialization;

        public SpecializationsController(ISpecialization specialization)
        {
            _specialization = specialization;
        }

        // GET: api/Specializations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations()
        {
            var specializations = await _specialization.GetAllSpecializationsAsync();

            if (specializations == null)
            {
                return NotFound();
            }

            return Ok(specializations);
        }
    }
}
