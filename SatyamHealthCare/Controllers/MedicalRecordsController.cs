using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.DTO;
using SatyamHealthCare.Models;
using SatyamHealthCare.IRepos;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IMedicalRecord medicalRecord1;

        public MedicalRecordsController(SatyamDbContext context, IMedicalRecord medicalRecord1)
        {
            _context = context;
            this.medicalRecord1 = medicalRecord1;
        }

        // GET: api/MedicalRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecordDTO>>> GetMedicalRecords()
        {
            var medicalRecords = await medicalRecord1.GetAllMedicalRecordsAsync();

            // Map MedicalRecord to MedicalRecordDTO
            var medicalRecordDtos = medicalRecords.Select(mr => new MedicalRecordDTO
            {
                PatientID = mr.PatientID,
                DoctorID = mr.DoctorID,
                ConsultationDateTime = mr.ConsultationDateTime,
                Diagnosis = mr.Diagnosis,
                PrescriptionID = mr.PrescriptionID,
                
            }).ToList();

            return Ok(medicalRecordDtos);
        }

        // GET: api/MedicalRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecordDTO>> GetMedicalRecord(int id)
        {
            var medicalRecord = await medicalRecord1.GetMedicalRecordByIdAsync(id);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            // Map MedicalRecord to MedicalRecordDTO
            var medicalRecordDto = new MedicalRecordDTO
            {
                PatientID = medicalRecord.PatientID,
                DoctorID = medicalRecord.DoctorID,
                ConsultationDateTime = medicalRecord.ConsultationDateTime,
                Diagnosis = medicalRecord.Diagnosis,
                PrescriptionID = medicalRecord.PrescriptionID,
                
            };

            return Ok(medicalRecordDto);
        }

        // PUT: api/MedicalRecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalRecord(int id, MedicalRecordDTO medicalRecordDto)
        {
            if (id != medicalRecordDto.PrescriptionID)
            {
                return BadRequest();
            }

            // Map MedicalRecordDTO to MedicalRecord
            var medicalRecord = new MedicalRecord
            {
                RecordID = id, // Keep the same record ID
                PatientID = medicalRecordDto.PatientID,
                DoctorID = medicalRecordDto.DoctorID,
                ConsultationDateTime = medicalRecordDto.ConsultationDateTime,
                Diagnosis = medicalRecordDto.Diagnosis,
                PrescriptionID = medicalRecordDto.PrescriptionID
            };

            try
            {
                await medicalRecord1.UpdateMedicalRecordAsync(medicalRecord);
            }
            catch (Exception)
            {
                if (!MedicalRecordExists(id))
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

        // POST: api/MedicalRecords
        [HttpPost]
        public async Task<ActionResult<MedicalRecordDTO>> PostMedicalRecord(MedicalRecordDTO medicalRecordDto)
        {
            // Map MedicalRecordDTO to MedicalRecord
            var medicalRecord = new MedicalRecord
            {
                PatientID = medicalRecordDto.PatientID,
                DoctorID = medicalRecordDto.DoctorID,
                ConsultationDateTime = medicalRecordDto.ConsultationDateTime,
                Diagnosis = medicalRecordDto.Diagnosis,
                PrescriptionID = medicalRecordDto.PrescriptionID
            };

            var createdMedicalRecord = await medicalRecord1.AddMedicalRecordAsync(medicalRecord);

            var createdMedicalRecordDto = new MedicalRecordDTO
            {
                PatientID = createdMedicalRecord.PatientID,
                DoctorID = createdMedicalRecord.DoctorID,
                ConsultationDateTime = createdMedicalRecord.ConsultationDateTime,
                Diagnosis = createdMedicalRecord.Diagnosis,
                PrescriptionID = createdMedicalRecord.PrescriptionID
            };

            return CreatedAtAction("GetMedicalRecord", new { id = createdMedicalRecord.RecordID }, createdMedicalRecordDto);
        }

        // DELETE: api/MedicalRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var medicalRecord = await medicalRecord1.GetMedicalRecordByIdAsync(id);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            await medicalRecord1.DeleteMedicalRecordAsync(id);

            return NoContent();
        }

        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.RecordID == id);
        }
    }
}
