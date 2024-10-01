using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IPrescription _prescriptionService;

        public PrescriptionsController(IPrescription prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        // GET: api/prescriptions
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetAllPrescriptions()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();

            // Map to DTOs
            var prescriptionDtos = prescriptions.Select(p => new PrescriptionDTO
            {
                PrescriptionID = p.PrescriptionID,
                NoOfDays = p.NoOfDays,
                Dosage = p.Dosage,
                BeforeAfterFood = p.BeforeAfterFood,
                Remark = p.Remark,
                MedicineIDs = p.PrescriptionMedicines.Select(pm => pm.MedicineID).ToList(),
                TestIDs = p.PrescriptionTests.Select(pt => pt.TestID).ToList()
            }).ToList();

            return Ok(prescriptionDtos);
        }

        // GET: api/prescriptions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescriptionById(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionById(id);
            if (prescription == null)
            {
                return NotFound();
            }

            var prescriptionDto = new PrescriptionDTO
            {
                PrescriptionID = prescription.PrescriptionID,
                NoOfDays = prescription.NoOfDays,
                Dosage = prescription.Dosage,
                BeforeAfterFood = prescription.BeforeAfterFood,
                Remark = prescription.Remark,
                MedicineIDs = prescription.PrescriptionMedicines.Select(pm => pm.MedicineID).ToList(),
                TestIDs = prescription.PrescriptionTests.Select(pt => pt.TestID).ToList()
            };

            return Ok(prescriptionDto);
        }

        // POST: api/prescriptions
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<ActionResult<PrescriptionDTO>> AddPrescription([FromBody] PrescriptionDTO prescriptionDto)
        {
            var prescription = new Prescription
            {
                NoOfDays = prescriptionDto.NoOfDays,
                Dosage = prescriptionDto.Dosage,
                BeforeAfterFood = prescriptionDto.BeforeAfterFood,
                Remark = prescriptionDto.Remark,
                PrescriptionMedicines = prescriptionDto.MedicineIDs
                    .Select(id => new PrescriptionMedicine { MedicineID = id }).ToList(),
                PrescriptionTests = prescriptionDto.TestIDs
                    .Select(id => new PrescriptionTest { TestID = id }).ToList(),
                AppointmentId = prescriptionDto.AppointmentId
            };

            await _prescriptionService.AddPrescriptionAsync(prescription);
            prescriptionDto.PrescriptionID = prescription.PrescriptionID;

            return CreatedAtAction(nameof(GetPrescriptionById), new { id = prescriptionDto.PrescriptionID }, prescriptionDto);
        }

       
        }

      




    }

