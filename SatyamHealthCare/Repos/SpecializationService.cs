using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SatyamHealthCare.Repositories
{
    public class SpecializationService : ISpecialization
    {
        private readonly SatyamDbContext _context;

        public SpecializationService(SatyamDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync()
        {
            return await _context.Specializations.Include(s => s.Doctors).ToListAsync();
        }
    }
}
