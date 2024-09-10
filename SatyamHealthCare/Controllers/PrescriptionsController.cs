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
    public class PrescriptionsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IPrescription prescription1;
        public PrescriptionsController(SatyamDbContext context, IPrescription prescription1)
        {
            _context = context;
            this.prescription1 = prescription1;
        }
        // GET: api/Prescriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions()
        {
            var prescriptions = await prescription1.GetAllPrescriptionsAsync();
            return Ok(prescriptions);
        }
        // GET: api/Prescriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Prescription>> GetPrescription(int id)
        {
            var prescription = await prescription1.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }
            return Ok(prescription);
        }
        // PUT: api/Prescriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrescription(int id, Prescription prescription)
        {
            if (id != prescription.PrescriptionID)
            {
                return BadRequest();
            }
            try
            {
                await prescription1.UpdatePrescriptionAsync(prescription);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await prescription1.GetPrescriptionByIdAsync(id) == null)
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
        // POST: api/Prescriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Prescription>> PostPrescription(Prescription prescription)
        {
            await prescription1.AddPrescriptionAsync(prescription);
            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.PrescriptionID },
           prescription);
        }
        // DELETE: api/Prescriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await prescription1.GetPrescriptionByIdAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }
            await prescription1.DeletePrescriptionAsync(id);
            return NoContent();
        }
        private bool PrescriptionExists(int id)
        {
            return _context.Prescriptions.Any(e => e.PrescriptionID == id);
        }
    }
}