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
using SatyamHealthCare.Exceptions;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Repos;
using static SatyamHealthCare.Constants.Status;

namespace SatyamHealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly SatyamDbContext _context;
        private readonly IAppointment appointment1;
        private readonly INotificationService notificationService1;

        public AppointmentsController(SatyamDbContext context, IAppointment appointment1, INotificationService notificationService1)
        {
            _context = context;
            this.appointment1 = appointment1;
            this.notificationService1 = notificationService1;
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
                throw new InvalidDoctorException("Invalid Doctor ID.");
            }

            var appointments = await appointment1.GetAppointmentsByDoctorId(doctorId);

            if (appointments == null || !appointments.Any())
            {
                throw new AppointmentNotFoundException("No appointments found for this doctor.");
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
                throw new AppointmentNotFoundException($"Appointment with ID {id} not found.");
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

        [Authorize(Roles = "Patient")]
        [HttpGet("patient/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsByPatient()
        {
            try
            {
                
                var patientIdClaim = User.FindFirst("PatientId");
                if (patientIdClaim == null)
                {
                    return Unauthorized("Patient ID not found in the token.");
                }

                if (!int.TryParse(patientIdClaim.Value, out int patientId))
                {
                    throw new PatientNotFoundException("Invalid Patient ID.");
                }

                
                var appointments = await appointment1.GetAppointmentsByPatientId(patientId);

                if (appointments == null || !appointments.Any())
                {
                    throw new AppointmentNotFoundException("No appointments found for this patient.");
                }

                var appointmentDtos = appointments.Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status
                }).ToList();

                return Ok(appointmentDtos);
            }
            catch (PatientNotFoundException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (AppointmentNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateAppointment(int id, AppointmentDTO appointmentDto)
        {
            try
            {
                var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (doctorIdClaim == null)
                {
                    return Unauthorized("Doctor ID not found in claims.");
                }

                var loggedInDoctorId = int.Parse(doctorIdClaim.Value);

                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == id);

                if (appointment == null)
                {
                    throw new AppointmentNotFoundException($"Appointment with ID {id} not found.");
                }

                if (appointment.DoctorId != loggedInDoctorId)
                {
                    return Forbid("You are not authorized to update this appointment.");
                }

                bool isRescheduled = appointment.AppointmentDate != appointmentDto.AppointmentDate ||
                                     appointment.AppointmentTime != appointmentDto.AppointmentTime; // Check if time is rescheduled

                appointment.AppointmentDate = appointmentDto.AppointmentDate;
                appointment.AppointmentTime = appointmentDto.AppointmentTime; // Update time
                appointment.Status = isRescheduled ? AppointmentStatus.Rescheduled : appointment.Status;

                var result = await _context.SaveChangesAsync() > 0;

                if (result && isRescheduled)
                {
                    var patient = await _context.Patients.FindAsync(appointment.PatientId);
                    var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);

                    if (patient != null && doctor != null)
                    {
                        await notificationService1.SendAppointmentRescheduleAsync(
                            patient.Email,
                            patient.ContactNumber,
                            patient.FullName,
                            doctor.FullName,
                            appointment.AppointmentDate ?? DateTime.Now,
                            appointment.AppointmentTime ?? TimeSpan.Zero // Pass the rescheduled time
                        );
                    }

                    return Ok(new { Message = "Appointment has been rescheduled and notification sent." });
                }

                return Ok(new { Message = "Appointment updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
            try
            {
                var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (doctorIdClaim == null)
                {
                    return Unauthorized("Doctor ID not found in the token.");
                }

                if (!int.TryParse(doctorIdClaim.Value, out int doctorId))
                {
                    throw new InvalidDoctorException("Invalid Doctor ID.");
                }

                Status.AppointmentStatus? status = null;
                if (!string.IsNullOrEmpty(statusString) && Enum.TryParse<Status.AppointmentStatus>(statusString, true, out var parsedStatus))
                {
                    status = parsedStatus;
                }

                var appointments = await appointment1.GetFilteredAppointmentsByDoctorId(doctorId, startDate, endDate, status);

                if (appointments == null || !appointments.Any())
                {
                    throw new AppointmentNotFoundException("No appointments found for this doctor with the given criteria.");
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
            catch (InvalidDoctorException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (AppointmentNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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


            bool isTimeSlotTaken = await _context.Appointments
                .AnyAsync(a => a.DoctorId == appointmentDto.DoctorId &&
                               a.AppointmentDate == appointmentDto.AppointmentDate &&
                               a.AppointmentTime == appointmentDto.AppointmentTime);

            if (isTimeSlotTaken)
            {

                return Conflict(new { Message = "The selected time slot is already booked for this doctor." });
            }


            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = appointmentDto.AppointmentDate,
                AppointmentTime = appointmentDto.AppointmentTime,
                Status = appointmentDto.Status
            };

            try
            {

                var createdAppointment = await appointment1.AddAppointment(appointment);


                var patient = await _context.Patients.FindAsync(appointmentDto.PatientId);
                var doctor = await _context.Doctors.FindAsync(appointmentDto.DoctorId);

                if (patient != null && doctor != null)
                {
                    await notificationService1.SendAppointmentConfirmationAsync(
                        patient.Email,
                        patient.ContactNumber,
                        patient.FullName,
                        doctor.FullName,
                        createdAppointment.AppointmentDate ?? DateTime.Now,
                        createdAppointment.AppointmentTime ?? TimeSpan.Zero
                    );
                }

                var response = new AppointmentResponseDTO
                {
                    AppointmentId = createdAppointment.AppointmentId,
                    Message = "Appointment is created successfully"
                };

                return CreatedAtAction(nameof(GetAppointment), new { id = createdAppointment.AppointmentId }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }


        // DELETE: api/Appointments/5
        [Authorize(Roles = "Patient,Doctor")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelAppointment(int id)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == id);

                if (appointment == null)
                {
                    throw new AppointmentNotFoundException($"Appointment with ID {id} not found.");
                }

                if (appointment.Status == AppointmentStatus.Cancelled)
                {
                    throw new AppointmentAlreadyCancelledException("Appointment is already cancelled.");
                }

                appointment.Status = AppointmentStatus.Cancelled;

                var result = await _context.SaveChangesAsync() > 0;

                if (result)
                {
                    var patient = await _context.Patients.FindAsync(appointment.PatientId);
                    var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);

                    if (patient != null && doctor != null)
                    {
                        await notificationService1.SendAppointmentCancellationAsync(
                            patient.Email,
                            patient.ContactNumber,
                            patient.FullName,
                            doctor.FullName,
                            appointment.AppointmentDate ?? DateTime.Now,
                            appointment.AppointmentTime ?? TimeSpan.Zero
                        );
                    }

                    return Ok(new { Message = "Appointment has been cancelled." });
                }

                return StatusCode(500, "Failed to cancel the appointment.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}