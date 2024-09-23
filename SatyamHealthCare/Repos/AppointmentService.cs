using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.DTO;
using SatyamHealthCare.Exceptions;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.Constants;

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

        public async Task<bool> UpdateAppointment(int id, UpdateAppointmentDTO updateDto)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                    throw new EntityNotFoundException("Appointment", id);
                }

            
            appointment.DoctorId = updateDto.DoctorId;
            appointment.AppointmentDate = updateDto.AppointmentDate;
            appointment.Status = updateDto.Status;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return true;
            }
            catch (Exception ex)
            {
                throw new EntityUpdateFailedException("Appointment", id, ex);
            }
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

       
    }



}

