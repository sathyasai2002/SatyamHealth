using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Repos
{
    public class TestService : ITest
    {
        private readonly SatyamDbContext _context;
        public TestService(SatyamDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Test>> GetAllTestsAsync()
        {
            return await _context.Tests.ToListAsync();
        }
        public async Task<Test> GetTestByIdAsync(int id)
        {
            return await _context.Tests.FindAsync(id);
        }
        public async Task AddTestAsync(Test test)
        {
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateTestAsync(Test test)
        {
            _context.Entry(test).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTestAsync(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test != null)
            {
                _context.Tests.Remove(test);
                await _context.SaveChangesAsync();
            }
        }
    }
}
