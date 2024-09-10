
    using SatyamHealthCare.Models;
    namespace SatyamHealthCare.IRepos
    {
        public interface IMedicalHistoryFile
        {
            MedicalHistoryFile GetById(int id);
            Task<IEnumerable<MedicalHistoryFile>> GetAllMedicalHistoryFiles();
            Task<MedicalHistoryFile> GetMedicalHistoryFileById(int id);
            Task<IEnumerable<MedicalHistoryFile>> GetMedicalHistoryFilesByPatientId(int patientId);
            Task AddMedicalHistoryFile(MedicalHistoryFile medicalHistoryFile);
            Task UpdateMedicalHistoryFile(MedicalHistoryFile medicalHistoryFile);
            Task DeleteMedicalHistoryFile(int id);
            Task<bool> MedicalHistoryFileExists(int id);
        }
    }
