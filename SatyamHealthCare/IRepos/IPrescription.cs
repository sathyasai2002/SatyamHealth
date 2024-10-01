using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
    public interface IPrescription
    {
        Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync();
        Task<Prescription?> GetPrescriptionById(int id);
        Task AddPrescriptionAsync(Prescription prescription);

    }
}
