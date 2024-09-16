using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.DTO;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repos;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IPatient patient1;

        public PatientsController(SatyamDbContext context,IPatient patient1)
            
        {
            _context = context;
            this.patient1 = patient1 ?? throw new ArgumentNullException(nameof(patient1));
        }
      [Authorize(Roles = "Admin")]
        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            var patients = await patient1.GetAllPatients();
            return Ok(patients);
        }

        // GET: api/Patients/5
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await patient1.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Patient")]

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            try
            {
                await patient1.UpdatePatient(patient);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Patient")]

        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(PatientDTO patientDto)
        {

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            var patient = new Patient
            {
                FullName = patientDto.FullName,
                DateOfBirth = patientDto.DateOfBirth,
                Gender = patientDto.Gender,
                BloodGroup = patientDto.BloodGroup,
                ContactNumber = patientDto.ContactNumber,
                Email = patientDto.Email,
                Address = patientDto.Address,
                Pincode = patientDto.Pincode,
                City = patientDto.City,
                Password = patientDto.Password,
                ProfilePicture = patientDto.ProfilePicture
            };
            var createdPatient = await patient1.AddPatient(patient);
            return Ok(new { message = "Patient registered successfully", patient = createdPatient });
        }

            // DELETE: api/Patients/5
            [HttpDelete("{id}")]
        [Authorize(Roles = "Patient")]

        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await patient1.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }

            var deleteResult = await patient1.DeletePatient(id);
            if (!deleteResult)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting patient");
            }

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientID == id);
        }
    }
}
