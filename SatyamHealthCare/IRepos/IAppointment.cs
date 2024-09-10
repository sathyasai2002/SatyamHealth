using SatyamHealthCare.DTO;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
        public interface IAppointment
        {
            Task<List<Appointment>> GetAllAppointments();
            Task<Appointment?> GetAppointmentById(int id);
            Task<Appointment> AddAppointment(Appointment appointment);
        Task<bool> UpdateAppointment(int id, UpdateAppointmentDTO updateDto);
            Task<bool> DeleteAppointment(int id);
            Task Save();
        
    }
}
