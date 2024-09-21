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
using SatyamHealthCare.Repos;
using System.Security.Claims;
using SatyamHealthCare.Exceptions;

namespace SatyamHealthCare.Controllers
{
   
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
            try
            {
                var prescription = await prescription1.GetPrescriptionByIdAsync(id);

                if (prescription == null)
                {
                    throw new PrescriptionNotFoundException($"Prescription with ID {id} was not found.");
                }

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
            catch (PrescriptionNotFoundException ex)
            {
                // Return a NotFound response with the exception message
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpGet ("Filter")]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetAllPrescriptionsForParticularPatient()
        {
            // Extract the patientId from the JWT claims
            var patientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (patientIdClaim == null)
            {
                return Unauthorized("Patient ID not found in the token.");
            }

            if (!int.TryParse(patientIdClaim.Value, out int patientId))
            {
                return BadRequest("Invalid Patient ID.");
            }

           
            var prescriptions = await _context.Prescriptions
                .Include(p => p.MedicalRecord) 
                .Where(p => p.MedicalRecord != null && p.MedicalRecord.PatientID == patientId)
                .ToListAsync();

            if (prescriptions == null || !prescriptions.Any())
            {
                return NotFound("No prescriptions found for this patient.");
            }

            var prescriptionDtos = prescriptions.Select(p => new PrescriptionDTO
            {
                PrescriptionID = p.PrescriptionID,
                MedicineName = p.MedicineName,
                NoOfDays = p.NoOfDays,
                Dosage = p.Dosage,
                BeforeAfterFood = p.BeforeAfterFood,
                MedicalRecord = new MedicalRecordDTO
                {
                    RecordID = p.MedicalRecord.RecordID,
                    PatientID = p.MedicalRecord.PatientID
                }
            }).ToList();

            return Ok(prescriptionDtos);
        }


        // PUT: api/Prescriptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescription(int id, PrescriptionDTO prescriptionDto)
        {
            if (id != prescriptionDto.PrescriptionID)
            {
                return BadRequest("Prescription ID mismatch.");
            }


            try
            {
                var prescription = await prescription1.GetPrescriptionByIdAsync(id);

                if (prescription == null)
                {
                    throw new PrescriptionNotFoundException($"Prescription with ID {id} was not found.");
                }

                var updatedPrescription = new Prescription
                {
                    PrescriptionID = prescriptionDto.PrescriptionID,
                    MedicineName = prescriptionDto.MedicineName,
                    NoOfDays = prescriptionDto.NoOfDays,
                    Dosage = prescriptionDto.Dosage,
                    BeforeAfterFood = prescriptionDto.BeforeAfterFood
                };

                await prescription1.UpdatePrescriptionAsync(updatedPrescription);

                return NoContent();
            }
            catch (PrescriptionNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

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

           
            prescriptionDto.PrescriptionID = prescription.PrescriptionID;

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.PrescriptionID }, prescriptionDto);
        }

        // DELETE: api/Prescriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            try
            {
                var prescription = await prescription1.GetPrescriptionByIdAsync(id);

                if (prescription == null)
                {
                    throw new PrescriptionNotFoundException($"Prescription with ID {id} was not found.");
                }

                await prescription1.DeletePrescriptionAsync(id);

                return NoContent();
            }
            catch (PrescriptionNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        private async Task<bool> PrescriptionExists(int id)
        {
            return await prescription1.GetPrescriptionByIdAsync(id) != null;
        }
    }
}
