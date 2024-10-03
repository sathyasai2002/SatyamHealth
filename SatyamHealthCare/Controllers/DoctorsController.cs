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
using SatyamHealthCare.Constants;

namespace SatyamHealthCare.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IDoctor doctor1;

        public DoctorsController(SatyamDbContext context, IDoctor doctor1)
        {
            _context = context;
            this.doctor1 = doctor1;
        }

        // GET: api/Doctors
        [Authorize(Roles = "Admin,Patient")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Select(d => new DoctorDTO
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    PhoneNo = d.PhoneNo,
                    Email = d.Email,
                    Password = d.Password,
                    Designation = d.Designation,
                    Experience = d.Experience,
                    SpecializationID = d.SpecializationID,
                    Qualification = d.Qualification
                })
                .ToListAsync();

            return Ok(doctors);
        }




        // GET: api/Doctors/5
        [Authorize(Roles = "Admin,Patient")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await doctor1.GetDoctorById(id);

            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor with ID {id} not found.");
            }

            return Ok(doctor);
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutDoctor(int id, [FromBody] DoctorDTO doctorDto)
        {
            if (id != doctorDto.DoctorId)
            {
                throw new ArgumentException("Provided doctor ID does not match the request.");
            }

            // Ensure AdminId is present and valid
            if (doctorDto.AdminId == null || !AdminExists(doctorDto.AdminId))
            {
                return BadRequest("Invalid or missing AdminId.");
            }

            // Map DTO to Doctor entity, including AdminId
            var doctor = new Doctor
            {
                DoctorId = doctorDto.DoctorId,
                FullName = doctorDto.FullName,
                PhoneNo = doctorDto.PhoneNo,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                Designation = doctorDto.Designation,
                Experience = doctorDto.Experience,
                SpecializationID = doctorDto.SpecializationID,
                Qualification = doctorDto.Qualification,
                AdminId = doctorDto.AdminId 
            };

            doctor1.UpdateDoctor(doctor);

            try
            {
                await doctor1.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    throw new DoctorNotFoundException($"Doctor with ID {id} not found.");
                }
                else
                {
                    throw new Exception("An error occurred while updating the doctor.");
                }
            }

            return NoContent();
        }



        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor([FromBody] DoctorDTO doctorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var doctor = new Doctor
            {
                FullName = doctorDto.FullName,
                PhoneNo = doctorDto.PhoneNo,
                Email = doctorDto.Email,
                Password = doctorDto.Password,
                Designation = doctorDto.Designation,
                Experience = doctorDto.Experience,
                SpecializationID = doctorDto.SpecializationID,
                Qualification = doctorDto.Qualification,
                
                AdminId = doctorDto.AdminId
            };
            var createdDoctor = await doctor1.AddDoctor(doctor);
            await doctor1.Save();


            return Ok(new { message = "Doctor registered successfully", doctor = createdDoctor });


        }

        // DELETE: api/Doctors/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await doctor1.GetDoctorById(id);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Doctor with ID {id} not found.");
            }

            await doctor1.DeleteDoctor(id);
            await doctor1.Save();

            return Ok();
        }
        [HttpGet("GetLoggedInDoctorName")]
        public async Task<IActionResult> GetLoggedInDoctorName()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            if (!int.TryParse(userId, out int doctorId))
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var doctor = await _context.Doctors
                    .Where(d => d.DoctorId == doctorId)
                    .Select(d => new { d.FullName, d.Email, }) 
                    .FirstOrDefaultAsync();

                if (doctor == null)
                {
                    return NotFound("Doctor not found.");
                }

                return Ok(new { FullName = doctor.FullName, Email = doctor.Email });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("GetDoctorAppointmentCounts")]
        public async Task<IActionResult> GetDoctorAppointmentCounts()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            if (!int.TryParse(userId, out int doctorId))
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var totalAppointments = await _context.Appointments
                    .CountAsync(a => a.DoctorId == doctorId);

                var pendingAppointments = await _context.Appointments
                    .CountAsync(a => a.DoctorId == doctorId && a.Status == Status.AppointmentStatus.Pending);

              
                var rescheduledAppointments = await _context.Appointments
                    .CountAsync(a => a.DoctorId == doctorId && a.Status == Status.AppointmentStatus.Rescheduled);

                var completedAppointments = await _context.Appointments
                    .CountAsync(a => a.DoctorId == doctorId && a.Status == Status.AppointmentStatus.Completed);

                var response = new
                {
                    TotalAppointments = totalAppointments,
                    PendingAppointments = pendingAppointments,
                    RescheduledAppointments = rescheduledAppointments,
                    CompletedAppointments = completedAppointments
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET: api/Doctors/GetTodayAppointments
        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet("GetTodayAppointments")]
        public async Task<IActionResult> GetTodayAppointments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            if (!int.TryParse(userId, out int doctorId))
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var today = DateTime.Today;

                var appointments = await _context.Appointments
                    .Where(a => a.AppointmentDate.HasValue && a.AppointmentDate.Value.Date == today && a.DoctorId == doctorId)
                    .Select(a => new
                    {
                        a.AppointmentId,
                        PatientName = _context.Patients.Where(p => p.PatientID == a.PatientId).Select(p => p.FullName).FirstOrDefault(),
                        a.AppointmentTime,
                        a.PatientId,
                        a.Status
                    })
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet ("Getting All Doctors")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsS()
        {
            var doctor = await doctor1.GetAllDoctors();
            return Ok(doctor);
        }
        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorId == id);
        }
        private bool AdminExists(int adminId)
        {
            return _context.Admins.Any(e => e.AdminId == adminId);
        }
    }
}
