
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
namespace SatyamHealthCare.Controllers
{
    [Authorize(Roles = "Doctor,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly ITest test1;
        public TestsController(SatyamDbContext context, ITest test1)
        {
            _context = context;
            this.test1 = test1;
        }
        // GET: api/Tests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetTests()
        {
            var tests = await test1.GetAllTestsAsync();
            return Ok(tests);
        }
        // GET: api/Tests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            var test = await test1.GetTestByIdAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            return Ok(test);
        }
        // PUT: api/Tests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTest(int id, Test test)
        {
            if (id != test.TestID)
            {
                return BadRequest();
            }
            try
            {
                await test1.UpdateTestAsync(test);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await test1.GetTestByIdAsync(id) == null)
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
        // POST: api/Tests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Test>> PostTest(Test test)
        {
            await test1.AddTestAsync(test);
            return CreatedAtAction(nameof(GetTest), new { id = test.TestID }, test);
        }
        // DELETE: api/Tests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = await test1.GetTestByIdAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            await test1.DeleteTestAsync(id);
            return NoContent();
        }
        private bool TestExists(int id)
        {
            return _context.Tests.Any(e => e.TestID == id);
        }
    }
}