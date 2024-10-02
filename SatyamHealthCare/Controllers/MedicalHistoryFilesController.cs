using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.DTO;
using SatyamHealthCare.Exceptions;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace SatyamHealthCare.Controllers
{
    [Authorize(Roles = "Admin,Patient,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoryFilesController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IMedicalHistoryFile _medicalHistoryFileRepo;

        public MedicalHistoryFilesController(SatyamDbContext context, IMedicalHistoryFile medicalHistoryFileRepo)
        {
            _context = context;
            _medicalHistoryFileRepo = medicalHistoryFileRepo;
        }

        // GET: api/MedicalHistoryFiles
        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistoryFile>>> GetMedicalHistoryFiles()
        {
            var medicalHistoryFiles = await _medicalHistoryFileRepo.GetAllMedicalHistoryFiles();
            return Ok(medicalHistoryFiles);
        }

        // GET: api/MedicalHistoryFiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalHistoryFile>> GetMedicalHistoryFile(int id)
        {
            var medicalHistoryFile = await _medicalHistoryFileRepo.GetMedicalHistoryFileById(id);
            if (medicalHistoryFile == null)
            {
                throw new MedicalHistoryFileNotFoundException($"Medical history file with ID {id} not found.");
            }
            return Ok(medicalHistoryFile);
        }

        // GET: api/MedicalHistoryFiles/patient/{patientId}

        [Authorize(Roles ="Doctor,Patient")]
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<MedicalHistoryFileDTO>> GetMedicalHistory(int patientId)
        {
            var medicalHistory = await _context.MedicalHistoryFiles
                .Where(m => m.PatientId == patientId)
                .ToListAsync();

            if (medicalHistory == null || !medicalHistory.Any())
            {
                return NotFound();
            }

            var firstRecord = medicalHistory.First(); 

            var medicalHistoryDTO = new MedicalHistoryFileDTO
            {
                MedicalHistoryId = firstRecord.MedicalHistoryId,
                PatientId = firstRecord.PatientId,
                HasChronicConditions = firstRecord.HasChronicConditions,
                ChronicConditions = firstRecord.ChronicConditions,
                HasAllergies = firstRecord.HasAllergies,
                Allergies = firstRecord.Allergies,
                TakesMedications = firstRecord.TakesMedications,
                Medications = firstRecord.Medications,
                HadSurgeries = firstRecord.HadSurgeries,
                Surgeries = firstRecord.Surgeries,
                HasFamilyHistory = firstRecord.HasFamilyHistory,
                FamilyHistory = firstRecord.FamilyHistory,
                HasLifestyleFactors = firstRecord.HasLifestyleFactors,
                LifestyleFactors = firstRecord.LifestyleFactors,
                VaccinationRecords = firstRecord.VaccinationRecords,
            };

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return new JsonResult(medicalHistoryDTO, options);
        }


        // PUT: api/MedicalHistoryFiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalHistoryFile(int id, [FromBody] MedicalHistoryFile medicalHistoryFile)
        {
            if (id != medicalHistoryFile.MedicalHistoryId)
            {
                throw new ArgumentException("The provided medical history file ID does not match the requested resource.");
            }

            var existingMedicalHistoryFile = await _medicalHistoryFileRepo.GetMedicalHistoryFileById(id);
            if (existingMedicalHistoryFile == null)
            {
                throw new MedicalHistoryFileNotFoundException($"Medical history file with ID {id} not found.");
            }

            await _medicalHistoryFileRepo.UpdateMedicalHistoryFile(medicalHistoryFile);
            return NoContent();
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> CreateMedicalHistory([FromBody] MedicalHistoryFileDTO medicalHistoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (patientIdClaim == null)
            {
                return Unauthorized("User not found");
            }

            int patientId = int.Parse(patientIdClaim.Value);

            var medicalHistory = new MedicalHistoryFile
            {
                PatientId = patientId,
                HasChronicConditions = medicalHistoryDTO.HasChronicConditions,
                ChronicConditions = medicalHistoryDTO.ChronicConditions,
                HasAllergies = medicalHistoryDTO.HasAllergies,
                Allergies = medicalHistoryDTO.Allergies,
                TakesMedications = medicalHistoryDTO.TakesMedications,
                Medications = medicalHistoryDTO.Medications,
                HadSurgeries = medicalHistoryDTO.HadSurgeries,
                Surgeries = medicalHistoryDTO.Surgeries,
                HasFamilyHistory = medicalHistoryDTO.HasFamilyHistory,
                FamilyHistory = medicalHistoryDTO.FamilyHistory,
                HasLifestyleFactors = medicalHistoryDTO.HasLifestyleFactors,
                LifestyleFactors = medicalHistoryDTO.LifestyleFactors,
                VaccinationRecords = medicalHistoryDTO.VaccinationRecords,
            };

            _context.MedicalHistoryFiles.Add(medicalHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicalHistoryFile), new { id = medicalHistory.MedicalHistoryId }, medicalHistory);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("my")]
        public async Task<ActionResult<MedicalHistoryFileDTO>> GetMyMedicalHistory()
        {
            var patientIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (patientIdClaim == null)
            {
                return Unauthorized("User not found");
            }

            int patientId = int.Parse(patientIdClaim.Value); 

            var medicalHistory = await _context.MedicalHistoryFiles
                .Where(m => m.PatientId == patientId)
                .ToListAsync();

            if (medicalHistory == null || !medicalHistory.Any())
            {
                return NotFound("No medical history found for this patient.");
            }

            var firstRecord = medicalHistory.First();

            var medicalHistoryDTO = new MedicalHistoryFileDTO
            {
                MedicalHistoryId = firstRecord.MedicalHistoryId,
                PatientId = firstRecord.PatientId,
                HasChronicConditions = firstRecord.HasChronicConditions,
                ChronicConditions = firstRecord.ChronicConditions,
                HasAllergies = firstRecord.HasAllergies,
                Allergies = firstRecord.Allergies,
                TakesMedications = firstRecord.TakesMedications,
                Medications = firstRecord.Medications,
                HadSurgeries = firstRecord.HadSurgeries,
                Surgeries = firstRecord.Surgeries,
                HasFamilyHistory = firstRecord.HasFamilyHistory,
                FamilyHistory = firstRecord.FamilyHistory,
                HasLifestyleFactors = firstRecord.HasLifestyleFactors,
                LifestyleFactors = firstRecord.LifestyleFactors,
                VaccinationRecords = firstRecord.VaccinationRecords,
            };

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return new JsonResult(medicalHistoryDTO, options);
        }


        [Authorize(Roles ="Patient")]
        [HttpGet("GetLoggedInPatientMedicalHistory")]
        public async Task<IActionResult> GetLoggedInPatientMedicalHistory()
        {
            var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "PatientId")?.Value;

            if (string.IsNullOrEmpty(patientIdClaim) || !int.TryParse(patientIdClaim, out int patientId))
            {
                return BadRequest("Valid Patient ID not found in claims.");
            }

            var medicalHistory = await _context.MedicalHistoryFiles
                .Where(m => m.PatientId == patientId)
                .ToListAsync();

            if (medicalHistory == null || !medicalHistory.Any())
            {
                return NotFound("No medical history found for this patient.");
            }

            return Ok(medicalHistory);
        }






        private bool MedicalHistoryFileExists(int id)
        {
            return _medicalHistoryFileRepo.GetById(id) != null;
        }
    }
}
