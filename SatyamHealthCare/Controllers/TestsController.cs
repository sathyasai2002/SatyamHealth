using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.DTO;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ITest _testService;

        public TestsController(ITest testService)
        {
            _testService = testService;
        }

        // GET: api/Tests
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDTO>>> GetTests()
        {
            var tests = await _testService.GetAllTestsAsync();

            // Mapping Test to TestDTO
            var testDtos = tests.Select(t => new TestDTO
            {
                TestID = t.TestID,
                TestName = t.TestName
            }).ToList();

            return Ok(testDtos);
        }

        // GET: api/Tests/5
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            var test = await _testService.GetTestByIdAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            return test;
        }

        // PUT: api/Tests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTest(int id, Test test)
        {
            if (id != test.TestID)
            {
                return BadRequest();
            }

            await _testService.UpdateTestAsync(test);

            return NoContent();
        }

        // POST: api/Tests
        [HttpPost]
        public async Task<ActionResult<Test>> PostTest(Test test)
        {
            await _testService.AddTestAsync(test);

            return CreatedAtAction("GetTest", new { id = test.TestID }, test);
        }

        // DELETE: api/Tests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = await _testService.GetTestByIdAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            await _testService.DeleteTestAsync(id);

            return NoContent();
        }

        private bool TestExists(int id)
        {
            return _testService.GetTestByIdAsync(id) != null;
        }
    }
}
