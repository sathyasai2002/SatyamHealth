using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.DTO;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

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
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> UpdateAppointment(int id, UpdateAppointmentDTO updateDto)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return false;
            }

            
            appointment.DoctorId = updateDto.DoctorId;
            appointment.AppointmentDate = updateDto.AppointmentDate;
            appointment.Status = updateDto.Status;

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }










    }
}
