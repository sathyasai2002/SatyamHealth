using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescribedTestsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IPrescribedTest prescribedTest1;
        public PrescribedTestsController(SatyamDbContext context, IPrescribedTest prescribedTest1)
        {
            _context = context;
            this.prescribedTest1 = prescribedTest1;
        }
        // GET: api/PrescribedTests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescribedTest>>> GetPrescribedTests()
        {
            var prescribedTests = await prescribedTest1.GetAllPrescribedTestsAsync();
            return Ok(prescribedTests);
        }
        // GET: api/PrescribedTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescribedTest>> GetPrescribedTest(int id)
        {
            var prescribedTest = await prescribedTest1.GetPrescribedTestByIdAsync(id);
            if (prescribedTest == null)
            {
                return NotFound();
            }
            return Ok(prescribedTest);
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
            try
            {
                await prescribedTest1.UpdatePrescribedTestAsync(prescribedTest);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PrescribedTestExists(id))
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
        public async Task<ActionResult<PrescribedTest>> PostPrescribedTest(PrescribedTest
       prescribedTest)
        {
            await prescribedTest1.AddPrescribedTestAsync(prescribedTest);
            return CreatedAtAction("GetPrescribedTest", new { id = prescribedTest.PrescribedTestID },
           prescribedTest);
        }
        // DELETE: api/PrescribedTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescribedTest(int id)
        {
            var prescribedTest = await prescribedTest1.GetPrescribedTestByIdAsync(id);
            if (prescribedTest == null)
            {
                return NotFound();
            }
            await prescribedTest1.DeletePrescribedTestAsync(id);
            return NoContent();
        }
        private async Task<bool> PrescribedTestExists(int id)
        {
            return await prescribedTest1.PrescribedTestExistsAsync(id);
        }
    }
}