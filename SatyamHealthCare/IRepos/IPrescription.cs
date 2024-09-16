using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
    public interface IPrescription
    {
        Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync();
        Task<Prescription> GetPrescriptionByIdAsync(int id);
        Task<Prescription?> GetPrescriptionById(int id, int patientId);        
        Task AddPrescriptionAsync(Prescription prescription);
        Task UpdatePrescriptionAsync(Prescription prescription);
        Task DeletePrescriptionAsync(int id);
    }
}
