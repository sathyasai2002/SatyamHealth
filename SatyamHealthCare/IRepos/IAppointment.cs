using SatyamHealthCare.Constants;
using SatyamHealthCare.DTO;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
        public interface IAppointment
        {
            Task<List<Appointment>> GetAllAppointments();
            Task<Appointment?> GetAppointmentById(int id);
            Task<Appointment> AddAppointment(Appointment appointment);
         Task<List<Appointment>> GetAppointmentsByDoctorId(int doctorId);

        Task<List<Appointment>> GetAppointmentsByPatientId(int patientId);
        Task<List<Appointment>> GetFilteredAppointmentsByDoctorId(int doctorId, DateTime? startDate, DateTime? endDate, Status.AppointmentStatus? status);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime, int doctorId);

        Task<bool> DeleteAppointment(int id);
            Task Save();
        
    }
}
