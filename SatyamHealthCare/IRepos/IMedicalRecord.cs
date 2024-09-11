using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
    public interface IMedicalRecord
    {
        Task<IEnumerable<MedicalRecord>> GetAllMedicalRecordsAsync();
        Task<MedicalRecord?> GetMedicalRecordByIdAsync(int id);
        Task<MedicalRecord> AddMedicalRecordAsync(MedicalRecord medicalRecord);
        Task UpdateMedicalRecordAsync(MedicalRecord medicalRecord);
        Task DeleteMedicalRecordAsync(int id);
    }
}
