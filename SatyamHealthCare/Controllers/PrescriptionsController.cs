using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.DTO;

namespace SatyamHealthCare.Controllers
{
    [Authorize(Roles = "Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IPrescription prescription1;

        public PrescriptionsController(SatyamDbContext context, IPrescription prescription1)
        {
            _context = context;
            this.prescription1 = prescription1;
        }

        // GET: api/Prescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetPrescriptions()
        {
            var prescriptions = await prescription1.GetAllPrescriptionsAsync();

            // Mapping Prescription to PrescriptionDTO
            var prescriptionDtos = prescriptions.Select(p => new PrescriptionDTO
            {
                PrescriptionID = p.PrescriptionID,
                MedicineName = p.MedicineName,
                NoOfDays = p.NoOfDays,
                Dosage = p.Dosage,
                BeforeAfterFood = p.BeforeAfterFood,
                MedicalRecord = p.MedicalRecord != null ? new MedicalRecordDTO
                {
                    PatientID = p.MedicalRecord.PatientID,
                    DoctorID = p.MedicalRecord.DoctorID,
                    ConsultationDateTime = p.MedicalRecord.ConsultationDateTime,
                    Diagnosis = p.MedicalRecord.Diagnosis,
                    PrescriptionID = p.MedicalRecord.PrescriptionID
                } : null
            }).ToList();

            return Ok(prescriptionDtos);
        }

        // GET: api/Prescriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescription(int id)
        {
            var prescription = await prescription1.GetPrescriptionByIdAsync(id);

            if (prescription == null)
            {
                return NotFound();
            }

            // Mapping Prescription to PrescriptionDTO
            var prescriptionDto = new PrescriptionDTO
            {
                PrescriptionID = prescription.PrescriptionID,
                MedicineName = prescription.MedicineName,
                NoOfDays = prescription.NoOfDays,
                Dosage = prescription.Dosage,
                BeforeAfterFood = prescription.BeforeAfterFood,
                MedicalRecord = prescription.MedicalRecord != null ? new MedicalRecordDTO
                {
                    PatientID = prescription.MedicalRecord.PatientID,
                    DoctorID = prescription.MedicalRecord.DoctorID,
                    ConsultationDateTime = prescription.MedicalRecord.ConsultationDateTime,
                    Diagnosis = prescription.MedicalRecord.Diagnosis,
                    PrescriptionID = prescription.MedicalRecord.PrescriptionID
                } : null
            };

            return Ok(prescriptionDto);
        }

        // PUT: api/Prescriptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescription(int id, PrescriptionDTO prescriptionDto)
        {
            if (id != prescriptionDto.PrescriptionID)
            {
                return BadRequest();
            }

            // Mapping PrescriptionDTO to Prescription
            var prescription = new Prescription
            {
                PrescriptionID = prescriptionDto.PrescriptionID,
                MedicineName = prescriptionDto.MedicineName,
                NoOfDays = prescriptionDto.NoOfDays,
                Dosage = prescriptionDto.Dosage,
                BeforeAfterFood = prescriptionDto.BeforeAfterFood
            };

            try
            {
                await prescription1.UpdatePrescriptionAsync(prescription);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PrescriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Prescriptions
        [HttpPost]
        public async Task<ActionResult<PrescriptionDTO>> PostPrescription(PrescriptionDTO prescriptionDto)
        {
            // Mapping PrescriptionDTO to Prescription
            var prescription = new Prescription
            {
                MedicineName = prescriptionDto.MedicineName,
                NoOfDays = prescriptionDto.NoOfDays,
                Dosage = prescriptionDto.Dosage,
                BeforeAfterFood = prescriptionDto.BeforeAfterFood
            };

            await prescription1.AddPrescriptionAsync(prescription);

            // Mapping back to PrescriptionDTO for the response
            prescriptionDto.PrescriptionID = prescription.PrescriptionID;

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.PrescriptionID }, prescriptionDto);
        }

        // DELETE: api/Prescriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await prescription1.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            await prescription1.DeletePrescriptionAsync(id);

            return NoContent();
        }

        private async Task<bool> PrescriptionExists(int id)
        {
            return await prescription1.GetPrescriptionByIdAsync(id) != null;
        }
    }
}
