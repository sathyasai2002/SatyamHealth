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

        [Authorize(Roles = "Doctor,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointments()
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                return Unauthorized("User email not found in the token.");
            }

            bool isAdmin = emailClaim.Value.Contains("admin", StringComparison.OrdinalIgnoreCase);

            if (isAdmin)
            {
                var appointments = await appointment1.GetAllAppointments();
                return Ok(appointments.Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status,
                    Symptoms = a.Symptoms,
                    PatientName = a.Patient?.FullName
                }).ToList());
            }
            else
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

                return Ok(appointments.Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status,
                    Symptoms = a.Symptoms,
                    PatientName = a.Patient?.FullName
                }).ToList());
            }
        }





        // GET: api/Appointments/5
        [Authorize(Roles = "Doctor,Patient,,Admin")]
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
                AppointmentTime = appointment.AppointmentTime,
                Status = appointment.Status,
                Symptoms = appointment.Symptoms
            };

            return Ok(appointmentDto);
        }

        [Authorize(Roles = "Patient,Admin")]
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
                    DoctorName = a.Doctor.FullName,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status,
                    Symptoms =a.Symptoms
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
        [Authorize(Roles = "Doctor,Patient")]
        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleAppointment(RescheduleAppointmentDTO rescheduleDTO)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == rescheduleDTO.AppointmentId);

            if (appointment == null)
            {
                return NotFound("Appointment not found");
            }

            if (appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Cancelled)
            {
                return BadRequest("Cannot reschedule a completed or cancelled appointment");
            }

         
            var isSlotBooked = await _context.Appointments
                .AnyAsync(a => a.AppointmentDate == rescheduleDTO.NewAppointmentDate.Value &&
                               a.AppointmentTime == rescheduleDTO.NewAppointmentTime.Value &&
                               a.AppointmentId != rescheduleDTO.AppointmentId && 
                               a.Status != AppointmentStatus.Cancelled);

            if (isSlotBooked)
            {
                return BadRequest("The selected time slot is already booked. Please choose a different time.");
            }

            appointment.AppointmentDate = rescheduleDTO.NewAppointmentDate.Value;
            appointment.AppointmentTime = rescheduleDTO.NewAppointmentTime.Value;
            appointment.Status = AppointmentStatus.Rescheduled;

            try
            {
                await _context.SaveChangesAsync();

                // Send notification to the patient
                await notificationService1.SendAppointmentRescheduleAsync(
                    appointment.Patient.Email,
                    appointment.Patient.ContactNumber,
                    appointment.Patient.FullName,
                    appointment.Doctor.FullName,
                    appointment.AppointmentDate.Value,
                    appointment.AppointmentTime.Value
                );

                return Ok(new { message = "Appointment rescheduled successfully and notification sent to the patient." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(rescheduleDTO.AppointmentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"An error occurred while rescheduling the appointment: {ex.Message}");
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
        [Authorize(Roles = "Patient,Admin")]
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
            if (!appointmentDto.AppointmentDate.HasValue)
            {
                return BadRequest("Appointment date is required.");
            }
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(appointmentDto.AppointmentDate.Value.ToUniversalTime(), istZone);

            istDate = istDate.Date;
            if (istDate < TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone).Date)
            {
                return BadRequest("Appointment date cannot be in the past.");
            }

            bool isTimeSlotTaken = await _context.Appointments
                .AnyAsync(a => a.DoctorId == appointmentDto.DoctorId &&
                               a.AppointmentDate == istDate &&
                               a.AppointmentTime == appointmentDto.AppointmentTime
                              );

            if (isTimeSlotTaken)
            {

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
        [Authorize(Roles = "Patient,Doctor,Admin")]
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

        [Authorize(Roles ="Doctor")]
        [HttpPut("{appointmentId}/status")]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, [FromBody] UpdatestatusDTO updateStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await appointment1.UpdateAppointmentStatusAsync(appointmentId, updateStatusDto.Status.ToString());
            if (!result)
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