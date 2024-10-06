using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
    public interface IPrescription
    {
        Task AddPrescriptionAsync(Prescription prescription);
        Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync();
        Task<IEnumerable<Prescription>> GetPrescriptionsByAppointmentIdAsync(int appointmentId);
        Task<Prescription> GetPrescriptionByIdAsync(int id);
    }
}