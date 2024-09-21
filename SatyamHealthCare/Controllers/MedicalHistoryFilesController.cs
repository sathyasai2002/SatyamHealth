using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.Exceptions;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
namespace SatyamHealthCare.Controllers
{
    [Authorize(Roles = "Admin,Patient,Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoryFiles1Controller : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IMedicalHistoryFile medicalHistoryFile1;
        public MedicalHistoryFiles1Controller(SatyamDbContext context, IMedicalHistoryFile
       medicalHistoryFile1)
        {
            _context = context;
            this.medicalHistoryFile1 = medicalHistoryFile1;
        }
        // GET: api/MedicalHistoryFiles1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistoryFile>>> GetMedicalHistoryFiles()
        {
            var medicalHistoryFiles = await medicalHistoryFile1.GetAllMedicalHistoryFiles();
            return Ok(medicalHistoryFiles);
        }
        // GET: api/MedicalHistoryFiles1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalHistoryFile>> GetMedicalHistoryFile(int id)
        {
            var medicalHistoryFile = await medicalHistoryFile1.GetMedicalHistoryFileById(id);
            if (medicalHistoryFile == null)
            {
                throw new MedicalHistoryFileNotFoundException($"Medical history file with ID {id} not found.");
            }
            return Ok(medicalHistoryFile);
        }
        // GET: api/MedicalHistoryFiles/patient/5
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalHistoryFile>>>
       GetMedicalHistoryFilesByPatientId(int patientId)
        {
            var medicalHistoryFiles = await
           medicalHistoryFile1.GetMedicalHistoryFilesByPatientId(patientId);
            if (medicalHistoryFiles == null)
            {
                throw new MedicalHistoryFileNotFoundException($"No medical history files found for patient ID {patientId}.");
            }
            return Ok(medicalHistoryFiles);
        }
        // PUT: api/MedicalHistoryFiles1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalHistoryFile(int id, MedicalHistoryFile
       medicalHistoryFile)
        {
            if (id != medicalHistoryFile.MedicalHistoryId)
            {
                throw new ArgumentException("The provided medical history file ID does not match the requested resource.");
            }
            var existingMedicalHistoryFile = await medicalHistoryFile1.GetMedicalHistoryFileById(id);
            if (existingMedicalHistoryFile == null)
            {
                throw new MedicalHistoryFileNotFoundException($"Medical history file with ID {id} not found.");
            }
            await medicalHistoryFile1.UpdateMedicalHistoryFile(medicalHistoryFile);
            return NoContent();
        }
        // POST: api/MedicalHistoryFiles1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicalHistoryFile>> PostMedicalHistoryFile(MedicalHistoryFile
       medicalHistoryFile)
        {
            await medicalHistoryFile1.AddMedicalHistoryFile(medicalHistoryFile);
            return CreatedAtAction(nameof(GetMedicalHistoryFile), new
            {
                id = medicalHistoryFile.MedicalHistoryId
            }, medicalHistoryFile);
        }
        // DELETE: api/MedicalHistoryFiles1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalHistoryFile(int id)
        {
            var medicalHistoryFile = await medicalHistoryFile1.GetMedicalHistoryFileById(id);
            if (medicalHistoryFile == null)
            {
                throw new MedicalHistoryFileNotFoundException($"Medical history file with ID {id} not found.");
            }
            await medicalHistoryFile1.DeleteMedicalHistoryFile(id);
            return NoContent();
        }
        private bool MedicalHistoryFileExists(int id)
        {
            return medicalHistoryFile1.GetById(id) != null;
        }
    }
}

