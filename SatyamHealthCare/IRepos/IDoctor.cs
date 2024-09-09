using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
    public interface IDoctor
    {
        Task<List<Doctor>> GetAllDoctors();
        Task<Doctor?> GetDoctorById(int id);
        Task <Doctor> AddDoctor(Doctor doctor);
        Task UpdateDoctor(Doctor doctor);
        Task<bool> DeleteDoctor(int id);
        Task Save();
    }
}
