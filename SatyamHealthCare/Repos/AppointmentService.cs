using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.DTO;
using SatyamHealthCare.Exceptions;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Constants;
using static SatyamHealthCare.Constants.Status;

namespace SatyamHealthCare.Repos
{
    public class AppointmentService:IAppointment
    {

        private readonly SatyamDbContext _context;

        public AppointmentService(SatyamDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAppointments()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentById(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
        }

        public async Task<Appointment> AddAppointment(Appointment appointment)
        {
            try
            {
                _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
            }
            catch (Exception ex)
            {
                throw new EntityAddFailedException("Appointment", ex);
            }
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime, int doctorId)
        {
         
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException($"Appointment with ID {appointmentId} not found.");
            }

         
            if (appointment.DoctorId != doctorId)
            {
                throw new UnauthorizedAccessException("You are not authorized to reschedule this appointment.");
            }

            appointment.AppointmentDate = newDate;
            appointment.AppointmentTime = newTime;
            appointment.Status = AppointmentStatus.Rescheduled;

            // Save changes to the database
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteAppointment(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    throw new EntityNotFoundException("Appointment", id);
                }

                _context.Appointments.Remove(appointment);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new EntityDeleteFailedException("Appointment", id, ex);
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }



        public async Task<List<Appointment>> GetAppointmentsByDoctorId(int doctorId)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientId(int patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetFilteredAppointmentsByDoctorId(int doctorId, DateTime? startDate, DateTime? endDate, Status.AppointmentStatus? status)
 {
     var query = _context.Appointments
         .Where(a => a.DoctorId == doctorId);

         if (startDate.HasValue)
         {
         query = query.Where(a => a.AppointmentDate >= startDate.Value);
         }

     if (endDate.HasValue)
     {
         query = query.Where(a => a.AppointmentDate <= endDate.Value);
     }

     if (status.HasValue)
     {
         query = query.Where(a => a.Status == status.Value);
     }

     return await query
         .Include(a => a.Patient)
         .OrderBy(a => a.AppointmentDate)
         .ToListAsync();
 }





        public async Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status)
        {
            
            if (!Enum.TryParse(typeof(Constants.Status.AppointmentStatus), status, true, out var appointmentStatus))
            {
                return false; 
            }

            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return false; 
            }
            
            appointment.Status = (Constants.Status.AppointmentStatus)appointmentStatus;

            
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return true; 
        }




        public async Task UpdateAppointmentStatusUsingHangfire(int appointmentId, AppointmentStatus newStatus)
        {
            Console.WriteLine($"Job started for Appointment ID: {appointmentId} at {DateTime.UtcNow}");
            try
            {
                var appointment = await _context.Appointments.FindAsync(appointmentId);
                if (appointment != null)
                {
                    appointment.Status = newStatus; // Update the status
                    await _context.SaveChangesAsync(); // Save the changes
                    Console.WriteLine($"Appointment ID {appointmentId} status updated to {newStatus}.");
                }
                else
                {
                    Console.WriteLine($"Appointment ID {appointmentId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework)
                Console.WriteLine($"Error updating appointment status: {ex.Message}");
            }
            Console.WriteLine($"Job finished for Appointment ID: {appointmentId} at {DateTime.UtcNow}");
        }




    }



}

