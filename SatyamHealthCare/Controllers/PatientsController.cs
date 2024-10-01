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
using SatyamHealthCare.Exceptions;
using System.Security.Claims;

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
        [Authorize(Roles = "Patient")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patientIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (patientIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            var loggedInPatientId = int.Parse(patientIdClaim.Value);

            if (loggedInPatientId != id)
            {
                return Forbid("You are not authorized to view this patient's details.");
            }

            var patient = await patient1.GetPatientById(id);

            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with ID {id} not found.");
            }

            return Ok(patient);
        }


        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Patient")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientUpdateDTO patientUpdateDTO)
        {
            if (id != patientUpdateDTO.PatientID)
            {
                return BadRequest("Patient ID mismatch");
            }

            var existingPatient = await patient1.GetPatientById(id);
            if (existingPatient == null)
            {
                return NotFound($"Patient with ID {id} not found");
            }

            // Update patient properties
            existingPatient.FullName = patientUpdateDTO.FullName;
            existingPatient.DateOfBirth = patientUpdateDTO.DateOfBirth;
            existingPatient.Gender = patientUpdateDTO.Gender;
            existingPatient.BloodGroup = patientUpdateDTO.BloodGroup;
            existingPatient.ContactNumber = patientUpdateDTO.ContactNumber;
            existingPatient.Email = patientUpdateDTO.Email;
            existingPatient.Address = patientUpdateDTO.Address;
            existingPatient.Pincode = patientUpdateDTO.Pincode;
            existingPatient.City = patientUpdateDTO.City;
            existingPatient.State = patientUpdateDTO.State;

            try
            {
                var updatedPatient = await patient1.UpdatePatient(existingPatient);
                return Ok(updatedPatient);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the patient");
            }
        }




        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(PatientDTO patientDto)
        {
            /*var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userId, out int patientId))
            {
                return BadRequest("Invalid user ID.");
            }*/
           

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
                State = patientDto.State,
                Password = patientDto.Password,
               // ProfilePicture = patientDto.ProfilePicture
            };
            try
            {
                var createdPatient = await patient1.AddPatient(patient);
                return Ok(new { message = "Patient registered successfully", patient = createdPatient });
            }
            catch (EntityAddFailedException ex)
            {
                // Log exception details here if necessary
                return StatusCode(500, new { message = "An error occurred while registering the patient.", details = ex.Message });
            }
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Patient")]

        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await patient1.GetPatientById(id);
            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with ID {id} not found.");
            }

            var deleteResult = await patient1.DeletePatient(id);
            if (!deleteResult)
            {
                throw new Exception("An error occurred while deleting the patient.");
            }

            return NoContent();
        }

        [HttpGet("GetLoggedInPatient")]
        public async Task<IActionResult> GetLoggedInPatient()
        {
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            // Parse the user ID to an integer
            if (!int.TryParse(userId, out int patientId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Fetch the patient from the database
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.PatientID == patientId);

            if (patient == null)
            {
                return NotFound("Patient not found.");
            }

            // Map the patient data to PatientDTO
            var patientDto = new PatientDTO
            {
                PatientID = patient.PatientID,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                BloodGroup = patient.BloodGroup,
                ContactNumber = patient.ContactNumber,
                Email = patient.Email,
                Address = patient.Address,
                Pincode = patient.Pincode,
                City = patient.City,
                State = patient.State,
              //  ProfilePicture = patient.ProfilePicture,
                
            };

            return Ok(patientDto);
        }


        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientID == id);
        }
    }
}
