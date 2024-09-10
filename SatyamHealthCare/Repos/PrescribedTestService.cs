
    using Microsoft.EntityFrameworkCore;
    using SatyamHealthCare.IRepos;
    using SatyamHealthCare.Models;
    namespace SatyamHealthCare.Repos
    {
        public class PrescribedTestService : IPrescribedTest
        {
            private readonly SatyamDbContext _context;
            public PrescribedTestService(SatyamDbContext context)
            {
                _context = context;
            }
            // Get all Prescribed Tests
            public async Task<IEnumerable<PrescribedTest>> GetAllPrescribedTestsAsync()
            {
                return await _context.PrescribedTests
                .Include(pt => pt.MedicalRecord)
                .Include(pt => pt.Test)
                .ToListAsync();
            }
            // Get a Prescribed Test by ID
            public async Task<PrescribedTest?> GetPrescribedTestByIdAsync(int id)
            {
                return await _context.PrescribedTests
                .Include(pt => pt.MedicalRecord)
                .Include(pt => pt.Test)
                .FirstOrDefaultAsync(pt => pt.PrescribedTestID == id);
            }
            // Add a new Prescribed Test
            public async Task AddPrescribedTestAsync(PrescribedTest prescribedTest)
            {
                await _context.PrescribedTests.AddAsync(prescribedTest);
                await _context.SaveChangesAsync();
            }
            // Update an existing Prescribed Test
            public async Task UpdatePrescribedTestAsync(PrescribedTest prescribedTest)
            {
                _context.Entry(prescribedTest).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            // Delete a Prescribed Test
            public async Task DeletePrescribedTestAsync(int id)
            {
                var prescribedTest = await _context.PrescribedTests.FindAsync(id);
                if (prescribedTest != null)
                {
                    _context.PrescribedTests.Remove(prescribedTest);
                    await _context.SaveChangesAsync();
                }
            }
            // Check if a Prescribed Test exists
            public async Task<bool> PrescribedTestExistsAsync(int id)
            {
                return await _context.PrescribedTests.AnyAsync(pt => pt.PrescribedTestID == id);
            }
        }
    }

