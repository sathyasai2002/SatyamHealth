using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly SatyamDbContext _context;

        public MedicalRecordsController(SatyamDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicalRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecords()
        {
            return await _context.MedicalRecords.ToListAsync();
        }

        // GET: api/MedicalRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecord(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return medicalRecord;
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

            _context.Entry(medicalRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
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
        public async Task<ActionResult<MedicalRecord>> PostMedicalRecord(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalRecord", new { id = medicalRecord.RecordID }, medicalRecord);
        }

        // DELETE: api/MedicalRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord == null)
            {
                return NotFound();
            }

            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalRecordExists(int id)
        {
            return _context.MedicalRecords.Any(e => e.RecordID == id);
        }
    }
}
