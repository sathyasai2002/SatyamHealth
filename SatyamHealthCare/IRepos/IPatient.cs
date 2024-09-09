using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
    public interface IPatient
    {
        Task<List<Patient>> GetAllPatients();
        Task<Patient?> GetPatientById(int id);
        Task<Patient> AddPatient(Patient patient);
        Task<Patient> UpdatePatient(Patient patient);
        Task<bool> DeletePatient(int id);
    }
}
