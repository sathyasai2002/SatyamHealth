
using SatyamHealthCare.Models;
namespace SatyamHealthCare.IRepos
    {
        public interface ITest
        {
            Task<IEnumerable<Test>> GetAllTestsAsync();
            Task<Test> GetTestByIdAsync(int id);
            Task AddTestAsync(Test test);
            Task UpdateTestAsync(Test test);
            Task DeleteTestAsync(int id);
        }
    }