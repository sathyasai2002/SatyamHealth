﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.DTO;
using SatyamHealthCare.Models;
using SatyamHealthCare.IRepos;
using Microsoft.AspNetCore.Authorization;
using SatyamHealthCare.Exceptions;

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
        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecordDTO>>> GetMedicalRecords()
        {
            var medicalRecords = await medicalRecord1.GetAllMedicalRecordsAsync();

            if (medicalRecords == null || !medicalRecords.Any())
            {
                throw new MedicalRecordNotFoundException("No medical records found.");
            }

            // Map MedicalRecord to MedicalRecordDTO
            var medicalRecordDtos = medicalRecords.Select(mr => new MedicalRecordDTO
            {
                PatientID = mr.PatientID,
                DoctorID = mr.DoctorID,
                Diagnosis = mr.Diagnosis,
                RecordID = mr.RecordID,
                PrescriptionID = mr.PrescriptionID,
                MedicalHistoryId = mr.MedicalHistoryId  
            }).ToList();

            return Ok(medicalRecordDtos);
        }

        // GET: api/MedicalRecords/5
        [Authorize(Roles = "Patient,Doctor,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecordDTO>> GetMedicalRecord(int id)
        {
            var medicalRecord = await medicalRecord1.GetMedicalRecordByIdAsync(id);

            if (medicalRecord == null)
            {
                throw new MedicalRecordNotFoundException($"Medical record with ID {id} not found.");
            }

            // Map MedicalRecord to MedicalRecordDTO
            var medicalRecordDto = new MedicalRecordDTO
            {
                PatientID = medicalRecord.PatientID,
                DoctorID = medicalRecord.DoctorID,
                Diagnosis = medicalRecord.Diagnosis,
                PrescriptionID = medicalRecord.PrescriptionID,
                MedicalHistoryId = medicalRecord.MedicalHistoryId  
            };

            return Ok(medicalRecordDto);
        }

        // PUT: api/MedicalRecords/5
        [Authorize(Roles = "Doctor,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalRecord(int id, MedicalRecordDTO medicalRecordDto)
        {
            if (id != medicalRecordDto.RecordID)
            {
                throw new ArgumentException("Provided medical record ID does not match the request.");
            }

            // Map MedicalRecordDTO to MedicalRecord
            var medicalRecord = new MedicalRecord
            {
                RecordID = id,
                PatientID = medicalRecordDto.PatientID,
                DoctorID = medicalRecordDto.DoctorID,
                Diagnosis = medicalRecordDto.Diagnosis,
                PrescriptionID = medicalRecordDto.PrescriptionID,
                MedicalHistoryId = medicalRecordDto.MedicalHistoryId  
            };

            try
            {
                await medicalRecord1.UpdateMedicalRecordAsync(medicalRecord);
            }
            catch (Exception)
            {
                if (!MedicalRecordExists(id))
                {
                    throw new MedicalRecordNotFoundException($"Medical record with ID {id} not found.");
                }
                else
                {
                    throw new Exception("An error occurred while updating the medical record.");
                }
            }

            return NoContent();
        }

        // POST: api/MedicalRecords
        [Authorize(Roles = "Doctor,Admin")]
        [HttpPost]
        public async Task<ActionResult<MedicalRecordDTO>> PostMedicalRecord(MedicalRecordDTO medicalRecordDto)
        {
            // Map MedicalRecordDTO to MedicalRecord
            var medicalRecord = new MedicalRecord
            {
                PatientID = medicalRecordDto.PatientID,
                DoctorID = medicalRecordDto.DoctorID,
                Diagnosis = medicalRecordDto.Diagnosis,
                PrescriptionID = medicalRecordDto.PrescriptionID,
                MedicalHistoryId = medicalRecordDto.MedicalHistoryId  
            };

            var createdMedicalRecord = await medicalRecord1.AddMedicalRecordAsync(medicalRecord);

            var createdMedicalRecordDto = new MedicalRecordDTO
            {
                PatientID = createdMedicalRecord.PatientID,
                DoctorID = createdMedicalRecord.DoctorID,
                Diagnosis = createdMedicalRecord.Diagnosis,
                PrescriptionID = createdMedicalRecord.PrescriptionID,
                MedicalHistoryId = createdMedicalRecord.MedicalHistoryId  
            };

            return CreatedAtAction("GetMedicalRecord", new { id = createdMedicalRecord.RecordID }, createdMedicalRecordDto);
        }

        // DELETE: api/MedicalRecords/5
        [Authorize(Roles = "Doctor,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var medicalRecord = await medicalRecord1.GetMedicalRecordByIdAsync(id);

            if (medicalRecord == null)
            {
                throw new MedicalRecordNotFoundException($"Medical record with ID {id} not found.");
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
