using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Hangfire;
using log4net;
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
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AppointmentsController(SatyamDbContext context, IAppointment appointment1, INotificationService notificationService1)
        {
            _context = context;
            this.appointment1 = appointment1;
            this.notificationService1 = notificationService1;
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointments()
        {
            _logger.Info("GetAppointments method called");
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email);
                if (emailClaim == null)
                {
                    _logger.Warn("User email not found in the token");
                    return Unauthorized("User email not found in the token.");
                }

                _logger.Debug($"User email: {emailClaim.Value}");
                bool isAdmin = emailClaim.Value.Contains("admin", StringComparison.OrdinalIgnoreCase);
                _logger.Info($"User is admin: {isAdmin}");

                if (isAdmin)
                {
                    _logger.Info("Retrieving all appointments for admin");
                    var appointments = await appointment1.GetAllAppointments();
                    _logger.Info($"Retrieved {appointments.Count()} appointments");

                    var appointmentDTOs = appointments.Select(a => new AppointmentDTO
                    {
                        AppointmentId = a.AppointmentId,
                        PatientId = a.PatientId,
                        DoctorId = a.DoctorId,
                        AppointmentDate = a.AppointmentDate,
                        AppointmentTime = a.AppointmentTime,
                        Status = a.Status,
                        Symptoms = a.Symptoms,
                        PatientName = a.Patient?.FullName
                    }).ToList();

                    _logger.Debug($"Returning {appointmentDTOs.Count} appointment DTOs");
                    return Ok(appointmentDTOs);
                }
                else
                {
                    _logger.Info("Retrieving appointments for doctor");
                    var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (doctorIdClaim == null)
                    {
                        _logger.Warn("Doctor ID not found in the token");
                        return Unauthorized("Doctor ID not found in the token.");
                    }

                    _logger.Debug($"Doctor ID claim: {doctorIdClaim.Value}");
                    if (!int.TryParse(doctorIdClaim.Value, out int doctorId))
                    {
                        _logger.Error($"Invalid Doctor ID: {doctorIdClaim.Value}");
                        throw new InvalidDoctorException("Invalid Doctor ID.");
                    }

                    _logger.Info($"Retrieving appointments for doctor ID: {doctorId}");
                    var appointments = await appointment1.GetAppointmentsByDoctorId(doctorId);

                    if (appointments == null || !appointments.Any())
                    {
                        _logger.Warn($"No appointments found for doctor ID: {doctorId}");
                        throw new AppointmentNotFoundException("No appointments found for this doctor.");
                    }

                    _logger.Info($"Retrieved {appointments.Count()} appointments for doctor ID: {doctorId}");
                    var appointmentDTOs = appointments.Select(a => new AppointmentDTO
                    {
                        AppointmentId = a.AppointmentId,
                        PatientId = a.PatientId,
                        DoctorId = a.DoctorId,
                        AppointmentDate = a.AppointmentDate,
                        AppointmentTime = a.AppointmentTime,
                        Status = a.Status,
                        Symptoms = a.Symptoms,
                        PatientName = a.Patient?.FullName
                    }).ToList();

                    _logger.Debug($"Returning {appointmentDTOs.Count} appointment DTOs for doctor ID: {doctorId}");
                    return Ok(appointmentDTOs);
                }
            }
            catch (InvalidDoctorException ex)
            {
                _logger.Error("Invalid doctor exception", ex);
                return BadRequest(ex.Message);
            }
            catch (AppointmentNotFoundException ex)
            {
                _logger.Warn("Appointment not found exception", ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error in GetAppointments", ex);
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }




        // GET: api/Appointments/5
        [Authorize(Roles = "Doctor,Patient,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            _logger.Info($"Attempting to retrieve appointment with ID: {id}");

            try
            {
                var appointment = await appointment1.GetAppointmentById(id);

                if (appointment == null)
                {
                    _logger.Warn($"Appointment with ID {id} not found.");
                    throw new AppointmentNotFoundException($"Appointment with ID {id} not found.");
                }

                var appointmentDto = new AppointmentDTO
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    Status = appointment.Status,
                    Symptoms = appointment.Symptoms
                };

                _logger.Info($"Successfully retrieved appointment with ID: {id}");
                return Ok(appointmentDto);
            }
            catch (AppointmentNotFoundException ex)
            {
                _logger.Warn($"Appointment not found: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving appointment with ID {id}: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Patient,Admin")]
        [HttpGet("patient/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointmentsByPatient()
        {
            _logger.Info("GetAppointmentsByPatient method called");
            try
            {
                var patientIdClaim = User.FindFirst("PatientId");
                if (patientIdClaim == null)
                {
                    _logger.Warn("Patient ID not found in the token");
                    return Unauthorized("Patient ID not found in the token.");
                }

                _logger.Debug($"Patient ID claim: {patientIdClaim.Value}");
                if (!int.TryParse(patientIdClaim.Value, out int patientId))
                {
                    _logger.Error($"Invalid Patient ID: {patientIdClaim.Value}");
                    throw new PatientNotFoundException("Invalid Patient ID.");
                }

                _logger.Info($"Retrieving appointments for patient ID: {patientId}");
                var appointments = await appointment1.GetAppointmentsByPatientId(patientId);

                if (appointments == null || !appointments.Any())
                {
                    _logger.Warn($"No appointments found for patient ID: {patientId}");
                    throw new AppointmentNotFoundException("No appointments found for this patient.");
                }

                _logger.Info($"Retrieved {appointments.Count()} appointments for patient ID: {patientId}");
                var appointmentDtos = appointments.Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status,
                    Symptoms = a.Symptoms
                }).ToList();

                _logger.Debug($"Returning {appointmentDtos.Count} appointment DTOs for patient ID: {patientId}");
                return Ok(appointmentDtos);
            }
            catch (PatientNotFoundException ex)
            {
                _logger.Error("Patient not found exception", ex);
                return BadRequest(new { Message = ex.Message });
            }
            catch (AppointmentNotFoundException ex)
            {
                _logger.Warn("Appointment not found exception", ex);
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error in GetAppointmentsByPatient", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Doctor,Patient,Admin")]
        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleAppointment(RescheduleAppointmentDTO rescheduleDTO)
        {
            _logger.Info($"RescheduleAppointment method called for AppointmentId: {rescheduleDTO.AppointmentId}");
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == rescheduleDTO.AppointmentId);

                if (appointment == null)
                {
                    _logger.Warn($"Appointment not found for AppointmentId: {rescheduleDTO.AppointmentId}");
                    return NotFound("Appointment not found");
                }

                _logger.Debug($"Current appointment status: {appointment.Status}");
                if (appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Cancelled)
                {
                    _logger.Warn($"Cannot reschedule appointment with status: {appointment.Status}");
                    return BadRequest("Cannot reschedule a completed or cancelled appointment");
                }

                _logger.Info($"Checking if new slot is available: Date: {rescheduleDTO.NewAppointmentDate}, Time: {rescheduleDTO.NewAppointmentTime}");
                var isSlotBooked = await _context.Appointments
                    .AnyAsync(a => a.AppointmentDate == rescheduleDTO.NewAppointmentDate.Value &&
                                   a.AppointmentTime == rescheduleDTO.NewAppointmentTime.Value &&
                                   a.AppointmentId != rescheduleDTO.AppointmentId &&
                                   a.Status != AppointmentStatus.Cancelled);

                if (isSlotBooked)
                {
                    _logger.Warn("The selected time slot is already booked");
                    return BadRequest("The selected time slot is already booked. Please choose a different time.");
                }

                _logger.Info("Updating appointment details");
                appointment.AppointmentDate = rescheduleDTO.NewAppointmentDate.Value;
                appointment.AppointmentTime = rescheduleDTO.NewAppointmentTime.Value;
                appointment.Status = AppointmentStatus.Rescheduled;

                try
                {
                    await _context.SaveChangesAsync();
                    _logger.Info("Appointment details updated successfully");

                    _logger.Info("Scheduling background job to update appointment status");
                    var jobId = BackgroundJob.Schedule(
                        () => appointment1.UpdateAppointmentStatusUsingHangfire(appointment.AppointmentId, AppointmentStatus.Pending),
                        appointment.AppointmentDate.Value.Date.Add(appointment.AppointmentTime.Value)
                    );
                    _logger.Debug($"Background job scheduled with ID: {jobId}");

                    _logger.Info("Sending appointment reschedule notification");
                    await notificationService1.SendAppointmentRescheduleAsync(
                        appointment.Patient.Email,
                        appointment.Patient.ContactNumber,
                        appointment.Patient.FullName,
                        appointment.Doctor.FullName,
                        appointment.AppointmentDate.Value,
                        appointment.AppointmentTime.Value
                    );
                    _logger.Info("Notification sent successfully");

                    return Ok(new { message = "Appointment rescheduled successfully and notification sent to the patient." });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!AppointmentExists(rescheduleDTO.AppointmentId))
                    {
                        _logger.Warn($"Appointment not found during update for AppointmentId: {rescheduleDTO.AppointmentId}", ex);
                        return NotFound();
                    }
                    else
                    {
                        _logger.Error("DbUpdateConcurrencyException occurred", ex);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("An error occurred while rescheduling the appointment", ex);
                    return StatusCode(500, $"An error occurred while rescheduling the appointment: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error in RescheduleAppointment", ex);
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
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
                    AppointmentTime = a.AppointmentTime,
                    PatientName = a.Patient?.FullName,
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
            _logger.Info($"Attempting to create appointment for patient {appointmentDto.PatientId}");

            if (appointmentDto == null)
            {
                _logger.Warn("Appointment DTO is null");
                return BadRequest("Appointment cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.Warn("Invalid model state");
                return BadRequest(ModelState);
            }

            if (!appointmentDto.AppointmentDate.HasValue)
            {
                _logger.Warn("Appointment date is missing");
                return BadRequest("Appointment date is required.");
            }

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(appointmentDto.AppointmentDate.Value.ToUniversalTime(), istZone);

            istDate = istDate.Date;
            if (istDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone).Date)
            {
                _logger.Warn($"Attempted to book appointment in the past: {istDate}");
                return BadRequest("Appointment date cannot be in the past.");
            }

            bool isTimeSlotTaken = await _context.Appointments
                .AnyAsync(a => a.DoctorId == appointmentDto.DoctorId &&
                               a.AppointmentDate == istDate &&
                               a.AppointmentTime == appointmentDto.AppointmentTime &&
                               a.Status != AppointmentStatus.Cancelled);

            if (isTimeSlotTaken)
            {
                _logger.Warn($"Time slot already taken for doctor {appointmentDto.DoctorId} on {istDate} at {appointmentDto.AppointmentTime}");
                return Conflict(new { Message = "The selected time slot is already booked for this doctor." });
            }

            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = istDate,
                AppointmentTime = appointmentDto.AppointmentTime,
                Status = appointmentDto.Status,
                Symptoms = appointmentDto.Symptoms
            };

            try
            {
                var createdAppointment = await appointment1.AddAppointment(appointment);
                _logger.Info($"Appointment created successfully with ID: {createdAppointment.AppointmentId}");

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
                    _logger.Info($"Confirmation notification sent for appointment ID: {createdAppointment.AppointmentId}");
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
                _logger.Error($"Error creating appointment: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Patient,Doctor,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelAppointment(int id)
        {
            _logger.Info($"Attempting to cancel appointment with ID: {id}");

            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == id);

                if (appointment == null)
                {
                    _logger.Warn($"Appointment with ID {id} not found.");
                    throw new AppointmentNotFoundException($"Appointment with ID {id} not found.");
                }

                if (appointment.Status == AppointmentStatus.Cancelled)
                {
                    _logger.Warn($"Attempted to cancel already cancelled appointment with ID: {id}");
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
                        _logger.Info($"Cancellation notification sent for appointment ID: {id}");
                    }

                    _logger.Info($"Appointment with ID {id} has been cancelled successfully.");
                    return Ok(new { Message = "Appointment has been cancelled." });
                }

                _logger.Error($"Failed to cancel appointment with ID: {id}");
                return StatusCode(500, "Failed to cancel the appointment.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error cancelling appointment: {ex.Message}", ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Doctor,Admin")]
        [HttpPut("{appointmentId}/status")]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, [FromBody] UpdatestatusDTO updateStatusDto)
        {
            _logger.Info($"Attempting to update status for appointment ID: {appointmentId}");

            if (!ModelState.IsValid)
            {
                _logger.Warn("Invalid model state for status update");
                return BadRequest(ModelState);
            }

            var result = await appointment1.UpdateAppointmentStatusAsync(appointmentId, updateStatusDto.Status.ToString());
            if (!result)
            {
                _logger.Warn($"Appointment with ID {appointmentId} not found for status update");
                return NotFound();
            }

            _logger.Info($"Status updated successfully for appointment ID: {appointmentId}");
            return NoContent();
        }


        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}