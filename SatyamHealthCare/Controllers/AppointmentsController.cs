using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.Constants;
using SatyamHealthCare.DTO;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repos;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IAppointment appointment1;

        public AppointmentsController(SatyamDbContext context, IAppointment appointment1)
        {
            _context = context;
            this.appointment1 = appointment1;
        }

        // GET: api/Appointments
        [Authorize(Roles = "Doctor")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointments()
        {
           
            var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in the token.");
            }

            if (!int.TryParse(doctorIdClaim.Value, out int doctorId))
            {
                return BadRequest("Invalid Doctor ID.");
            }

            var appointments = await appointment1.GetAppointmentsByDoctorId(doctorId);

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found for this doctor.");
            }

            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status
            }).ToList();

            return Ok(appointmentDtos);
        }

 
        // GET: api/Appointments/5
        [Authorize(Roles = "Doctor,Patient")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await appointment1.GetAppointmentById(id);

            if (appointment == null)
            {
                return NotFound();
            }

            var appointmentDto = new AppointmentDTO
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentDate = appointment.AppointmentDate,
                Status = appointment.Status
            };

            return Ok(appointmentDto);
        }
        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] UpdateAppointmentDTO updateDto)
        {
            if (id != updateDto.AppointmentId)
            {
                return BadRequest();
            }

            var updated = await appointment1.UpdateAppointment(id, updateDto); 

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Doctor")]
        [HttpGet("filtered")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsByFiltering(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string statusString)
        {
            var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (doctorIdClaim == null)
            {
                return Unauthorized("Doctor ID not found in the token.");
            }

            if (!int.TryParse(doctorIdClaim.Value, out int doctorId))
            {
                return BadRequest("Invalid Doctor ID.");
            }

            Status.AppointmentStatus? status = null;
            if (!string.IsNullOrEmpty(statusString) && System.Enum.TryParse<Status.AppointmentStatus>(statusString, true, out var parsedStatus))
            {
                status = parsedStatus;
            }

            var appointments = await appointment1.GetFilteredAppointmentsByDoctorId(doctorId, startDate, endDate, status);

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found for this doctor with the given criteria.");
            }

            var appointmentDtos = appointments.Select(a => new AppointmentDTO
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status
            }).ToList();

            return Ok(appointmentDtos);
        }
        [Authorize(Roles = "Patient")]
        [HttpPost]
       
        public async Task<ActionResult<Appointment>> PostAppointment(AppointmentDTO appointmentDto)
        {
            if (appointmentDto == null)
            {
                return BadRequest("Appointment cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = appointmentDto.Status
            };
            try
            {
                var createdAppointment = await appointment1.AddAppointment(appointment);
                return CreatedAtAction(nameof(GetAppointment), new { id = createdAppointment.AppointmentId }, createdAppointment);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Appointments/5
        [Authorize(Roles = "Patient,Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var deleted = await appointment1.DeleteAppointment(id); 

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}
