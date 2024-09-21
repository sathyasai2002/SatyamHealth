﻿using System;
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
using SatyamHealthCare.Exceptions;

namespace SatyamHealthCare.Controllers
{
    [Authorize(Roles = "Doctor")]

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
        public async Task<ActionResult<IEnumerable<PrescribedTestDTO>>> GetPrescribedTests()
        {
            var prescribedTests = await prescribedTest1.GetAllPrescribedTestsAsync();

            if (prescribedTests == null || !prescribedTests.Any())
            {
                throw new PrescribedTestNotFoundException("No prescribed tests found.");
            }

            // Mapping PrescribedTest to PrescribedTestDTO
            var prescribedTestDtos = prescribedTests.Select(pt => new PrescribedTestDTO
            {
                PrescribedTestID = pt.PrescribedTestID,
                RecordID = pt.RecordID,
                TestID = pt.TestID,
                TestResult = pt.TestResult,
                
            }).ToList();

            return Ok(prescribedTestDtos);
        }

        // GET: api/PrescribedTests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescribedTestDTO>> GetPrescribedTest(int id)
        {
            var prescribedTest = await prescribedTest1.GetPrescribedTestByIdAsync(id);

            if (prescribedTest == null)
            {
                throw new PrescribedTestNotFoundException($"Prescribed test with ID {id} not found.");
            }

            // Mapping PrescribedTest to PrescribedTestDTO
            var prescribedTestDto = new PrescribedTestDTO
            {
                PrescribedTestID = prescribedTest.PrescribedTestID,
                RecordID = prescribedTest.RecordID,
                TestID = prescribedTest.TestID,
                TestResult = prescribedTest.TestResult,
                
            };

            return Ok(prescribedTestDto);
        }

        // PUT: api/PrescribedTests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescribedTest(int id, PrescribedTestDTO prescribedTestDto)
        {
            if (id != prescribedTestDto.PrescribedTestID)
            {
                throw new ArgumentException("Provided prescribed test ID does not match the request.");
            }

            // Mapping PrescribedTestDTO to PrescribedTest
            var prescribedTest = new PrescribedTest
            {
                PrescribedTestID = prescribedTestDto.PrescribedTestID,
                RecordID = prescribedTestDto.RecordID,
                TestID = prescribedTestDto.TestID,
                TestResult = prescribedTestDto.TestResult
            };

            try
            {
                await prescribedTest1.UpdatePrescribedTestAsync(prescribedTest);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PrescribedTestExists(id))
                {
                    throw new PrescribedTestNotFoundException($"Prescribed test with ID {id} not found.");
                }
                else
                {
                    throw new Exception("An error occurred while updating the prescribed test.");
                }
            }

            return NoContent();
        }

        // POST: api/PrescribedTests
        [HttpPost]
        public async Task<ActionResult<PrescribedTestDTO>> PostPrescribedTest(PrescribedTestDTO prescribedTestDto)
        {
            // Mapping PrescribedTestDTO to PrescribedTest
            var prescribedTest = new PrescribedTest
            {
                RecordID = prescribedTestDto.RecordID,
                TestID = prescribedTestDto.TestID,
                TestResult = prescribedTestDto.TestResult
            };

            await prescribedTest1.AddPrescribedTestAsync(prescribedTest);

            // Mapping back to PrescribedTestDTO for the response
            prescribedTestDto.PrescribedTestID = prescribedTest.PrescribedTestID;

            return CreatedAtAction("GetPrescribedTest", new { id = prescribedTest.PrescribedTestID }, prescribedTestDto);
        }

        // DELETE: api/PrescribedTests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescribedTest(int id)
        {
            var prescribedTest = await prescribedTest1.GetPrescribedTestByIdAsync(id);
            if (prescribedTest == null)
            {
                throw new PrescribedTestNotFoundException($"Prescribed test with ID {id} not found.");
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
