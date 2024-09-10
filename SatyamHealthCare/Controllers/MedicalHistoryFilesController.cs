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
    public class MedicalHistoryFilesController : ControllerBase
    {
        private readonly SatyamDbContext _context;

        public MedicalHistoryFilesController(SatyamDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicalHistoryFiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistoryFile>>> GetMedicalHistoryFiles()
        {
            return await _context.MedicalHistoryFiles.ToListAsync();
        }

        // GET: api/MedicalHistoryFiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalHistoryFile>> GetMedicalHistoryFile(int id)
        {
            var medicalHistoryFile = await _context.MedicalHistoryFiles.FindAsync(id);

            if (medicalHistoryFile == null)
            {
                return NotFound();
            }

            return medicalHistoryFile;
        }

        // PUT: api/MedicalHistoryFiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalHistoryFile(int id, MedicalHistoryFile medicalHistoryFile)
        {
            if (id != medicalHistoryFile.MedicalHistoryId)
            {
                return BadRequest();
            }

            _context.Entry(medicalHistoryFile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalHistoryFileExists(id))
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

        // POST: api/MedicalHistoryFiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicalHistoryFile>> PostMedicalHistoryFile(MedicalHistoryFile medicalHistoryFile)
        {
            _context.MedicalHistoryFiles.Add(medicalHistoryFile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedicalHistoryFile", new { id = medicalHistoryFile.MedicalHistoryId }, medicalHistoryFile);
        }

        // DELETE: api/MedicalHistoryFiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalHistoryFile(int id)
        {
            var medicalHistoryFile = await _context.MedicalHistoryFiles.FindAsync(id);
            if (medicalHistoryFile == null)
            {
                return NotFound();
            }

            _context.MedicalHistoryFiles.Remove(medicalHistoryFile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalHistoryFileExists(int id)
        {
            return _context.MedicalHistoryFiles.Any(e => e.MedicalHistoryId == id);
        }
    }
}
