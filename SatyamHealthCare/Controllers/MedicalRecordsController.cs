using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecords()
        {
            var medicalRecords = await medicalRecord1.GetAllMedicalRecordsAsync();
            return Ok(medicalRecords);
        }
        // GET: api/MedicalRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecord(int id)
        {
            var medicalRecord = await medicalRecord1.GetMedicalRecordByIdAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }
            return Ok(medicalRecord);
        }
        // PUT: api/MedicalRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalRecord(int id, MedicalRecord medicalRecord)
        {
            if (id != medicalRecord.RecordID)
            {
                return BadRequest();
            }
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicalRecord>> PostMedicalRecord(MedicalRecord
       medicalRecord)
        {
            await medicalRecord1.AddMedicalRecordAsync(medicalRecord);
            return CreatedAtAction("GetMedicalRecord", new { id = medicalRecord.RecordID },
           medicalRecord);
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