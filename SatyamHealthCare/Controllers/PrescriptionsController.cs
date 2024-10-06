using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.DTOs;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repos;

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

        // POST: api/prescriptions
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<ActionResult<PrescriptionDTO>> AddPrescription([FromBody] PrescriptionDTO prescriptionDto)
        {
         
            
            var prescription = new Prescription
            {
                Remark = prescriptionDto.Remark,
                AppointmentId = prescriptionDto.AppointmentId,
                PrescriptionMedicines = prescriptionDto.Medicines.Select(m => new PrescriptionMedicine
                {
                    MedicineID = m.MedicineID,
                    Dosage = m.Dosage,
                    DosageUnit=m.DosageUnit,
                    NoOfDays = m.NoOfDays,
                    BeforeAfterFood = m.BeforeAfterFood,
                    DosageFrequency = m.DosageFrequency
                }).ToList(),
            PrescriptionTests = prescriptionDto.TestIDs.Select(id => new PrescriptionTest
            {
                TestID = id
            }).ToList()
            };
            
            await _prescriptionService.AddPrescriptionAsync(prescription);
            prescriptionDto.PrescriptionID = prescription.PrescriptionID;

            return CreatedAtAction(nameof(GetPrescriptionById), new { id = prescriptionDto.PrescriptionID }, prescriptionDto);
        }

        // GET: api/prescriptions
        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetAllPrescriptions()
        {
            var prescriptions = await _prescriptionService.GetAllPrescriptionsAsync();

            // Map to DTOs
            var prescriptionDtos = prescriptions.Select(p => new PrescriptionDTO
            {
                PrescriptionID = p.PrescriptionID,
                Remark = p.Remark,
                AppointmentId = p.AppointmentId,
                Medicines = p.PrescriptionMedicines.Select(pm => new MedicineDTO
                {
                    MedicineID = pm.MedicineID,
                    Dosage = pm.Dosage,
                    DosageUnit = pm.DosageUnit,
                    NoOfDays = pm.NoOfDays,
                    DosageFrequency = pm.DosageFrequency,
                    BeforeAfterFood = pm.BeforeAfterFood
                }).ToList(),
                TestIDs = p.PrescriptionTests.Select(pt => pt.TestID).ToList()
            }).ToList();

            return Ok(prescriptionDtos);
        }

        [Authorize]
        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetPrescriptionsByAppointmentId(int appointmentId)
        {
            var prescriptions = await _prescriptionService.GetPrescriptionsByAppointmentIdAsync(appointmentId) ?? new List<Prescription>();

            // Map to DTOs
            var prescriptionDtos = prescriptions.Select(p => new PrescriptionDTO
            {
                PrescriptionID = p.PrescriptionID,
                Remark = p.Remark,
                AppointmentId = p.AppointmentId,
                Medicines = p.PrescriptionMedicines?.Select(pm => new MedicineDTO
                {
                    MedicineID = pm.MedicineID,
                    Dosage = pm.Dosage,
                    DosageUnit = pm.DosageUnit,
                    NoOfDays = pm.NoOfDays,
                    DosageFrequency = pm.DosageFrequency,
                    BeforeAfterFood = pm.BeforeAfterFood
                }).ToList() ?? new List<MedicineDTO>(), // Handle null case for Medicines
                TestIDs = p.PrescriptionTests?.Select(pt => pt.TestID).ToList() ?? new List<int>() // Handle null case for TestIDs
            }).ToList();

            return Ok(prescriptionDtos);
        }




        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescriptionById(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            var prescriptionDto = new PrescriptionDTO
            {
                PrescriptionID = prescription.PrescriptionID,
                Remark = prescription.Remark,
                AppointmentId = prescription.AppointmentId,
                Medicines = prescription.PrescriptionMedicines.Select(pm => new MedicineDTO
                {
                    MedicineID = pm.MedicineID,
                    Dosage = pm.Dosage,
                    DosageUnit = pm.DosageUnit,
                    NoOfDays = pm.NoOfDays,
                    DosageFrequency = pm.DosageFrequency,
                    BeforeAfterFood = pm.BeforeAfterFood
                }).ToList(),
                TestIDs = prescription.PrescriptionTests.Select(pt => pt.TestID).ToList() // Include TestIDs in the DTO
            };

            return Ok(prescriptionDto);
        }
    }
}
