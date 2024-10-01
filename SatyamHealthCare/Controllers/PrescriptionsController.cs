using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.DTOs;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IPrescription _prescriptionService;

        public PrescriptionsController(SatyamDbContext context, IPrescription prescriptionService)
        {
            _context = context;
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetAllPrescriptions()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();
            // Map to DTOs
            var prescriptionDtos = prescriptions.Select(p => new PrescriptionDTO
            {
                PrescriptionID = p.PrescriptionID,
                MedicineID = p.MedicineID,
                TestID = p.TestID,
                MedicineName = p.Medicine?.MedicineName, // Accessing MedicineName via navigation property
                NoOfDays = p.NoOfDays,
                Dosage = p.Dosage,
                BeforeAfterFood = p.BeforeAfterFood,
                Remark = p.Remark
            }).ToList();

            return Ok(prescriptionDtos);
        }

        // GET: api/prescription/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescriptionById(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionById(id);
            if (prescription == null)
            {
                return NotFound();
            }
            // Map to DTO
            var prescriptionDto = new PrescriptionDTO
            {
                PrescriptionID = prescription.PrescriptionID,
                MedicineID = prescription.MedicineID,
                TestID = prescription.TestID,
                MedicineName = prescription.Medicine?.MedicineName,
                NoOfDays = prescription.NoOfDays,
                Dosage = prescription.Dosage,
                BeforeAfterFood = prescription.BeforeAfterFood,
                Remark = prescription.Remark
            };

            return Ok(prescriptionDto);
        }

        // POST: api/prescription
        [Authorize(Roles ="Doctor")]
        
        [HttpPost]
        public async Task<ActionResult<PrescriptionDTO>> AddPrescription(PrescriptionDTO prescriptionDto)
        {
            var prescription = new Prescription
            {
                // Map properties from DTO to the entity
                MedicineID = prescriptionDto.MedicineID,
                TestID = prescriptionDto.TestID,
                NoOfDays = prescriptionDto.NoOfDays,
                Dosage = prescriptionDto.Dosage,
                BeforeAfterFood = prescriptionDto.BeforeAfterFood,
                Remark = prescriptionDto.Remark
            };

            await _prescriptionService.AddPrescriptionAsync(prescription);
            prescriptionDto.PrescriptionID = prescription.PrescriptionID; // Set the created ID to DTO

            return CreatedAtAction(nameof(GetPrescriptionById), new { id = prescriptionDto.PrescriptionID }, prescriptionDto);
        }
    }
}
