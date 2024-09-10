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
    public class PrescribedTestsController : ControllerBase
    {
        private readonly SatyamDbContext _context;

        public PrescribedTestsController(SatyamDbContext context)
        {
            _context = context;
        }

        // GET: api/PrescribedTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescribedTest>>> GetPrescribedTests()
        {
            return await _context.PrescribedTests.ToListAsync();
        }

        // GET: api/PrescribedTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescribedTest>> GetPrescribedTest(int id)
        {
            var prescribedTest = await _context.PrescribedTests.FindAsync(id);

            if (prescribedTest == null)
            {
                return NotFound();
            }

            return prescribedTest;
        }

        // PUT: api/PrescribedTests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescribedTest(int id, PrescribedTest prescribedTest)
        {
            if (id != prescribedTest.PrescribedTestID)
            {
                return BadRequest();
            }

            _context.Entry(prescribedTest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrescribedTestExists(id))
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

        // POST: api/PrescribedTests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PrescribedTest>> PostPrescribedTest(PrescribedTest prescribedTest)
        {
            _context.PrescribedTests.Add(prescribedTest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrescribedTest", new { id = prescribedTest.PrescribedTestID }, prescribedTest);
        }

        // DELETE: api/PrescribedTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescribedTest(int id)
        {
            var prescribedTest = await _context.PrescribedTests.FindAsync(id);
            if (prescribedTest == null)
            {
                return NotFound();
            }

            _context.PrescribedTests.Remove(prescribedTest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrescribedTestExists(int id)
        {
            return _context.PrescribedTests.Any(e => e.PrescribedTestID == id);
        }
    }
}
