using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
    public interface IPrescribedTest
    {
        Task<IEnumerable<PrescribedTest>> GetAllPrescribedTestsAsync();
        Task<PrescribedTest?> GetPrescribedTestByIdAsync(int id);
        Task AddPrescribedTestAsync(PrescribedTest prescribedTest);
        Task UpdatePrescribedTestAsync(PrescribedTest prescribedTest);
        Task DeletePrescribedTestAsync(int id);
        Task<bool> PrescribedTestExistsAsync(int id);
    }

}
