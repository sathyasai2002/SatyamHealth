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
using SatyamHealthCare.DTO;

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
        public async Task<ActionResult<IEnumerable<TestDTO>>> GetTests()
        {
            var tests = await test1.GetAllTestsAsync();

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
        public async Task<ActionResult<TestDTO>> GetTest(int id)
        {
            var test = await test1.GetTestByIdAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            // Mapping Test to TestDTO
            var testDto = new TestDTO
            {
                TestID = test.TestID,
                TestName = test.TestName
            };

            return Ok(testDto);
        }

        // PUT: api/Tests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTest(int id, TestDTO testDto)
        {
            if (id != testDto.TestID)
            {
                return BadRequest();
            }

            // Mapping TestDTO to Test
            var test = new Test
            {
                TestID = testDto.TestID,
                TestName = testDto.TestName
            };

            try
            {
                await test1.UpdateTestAsync(test);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TestExists(id))
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
        [HttpPost]
        public async Task<ActionResult<TestDTO>> PostTest(TestDTO testDto)
        {
            // Mapping TestDTO to Test
            var test = new Test
            {
                TestName = testDto.TestName
            };

            await test1.AddTestAsync(test);

            // Mapping back to TestDTO for the response
            testDto.TestID = test.TestID;

            return CreatedAtAction(nameof(GetTest), new { id = test.TestID }, testDto);
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

        private async Task<bool> TestExists(int id)
        {
            return await test1.GetTestByIdAsync(id) != null;
        }
    }
}
