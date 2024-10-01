using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.DTOs; // Make sure to include the DTO namespace
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SatyamHealthCare.Repos
{
    public class PrescriptionService : IPrescription
    {
        private readonly SatyamDbContext _context;

        public PrescriptionService(SatyamDbContext context)
        {
            _context = context;
        }

        // Get all prescriptions
        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.Medicine)
                .Include(p => p.Test)
                .ToListAsync();
        }

        // Get a prescription by ID
        public async Task<Prescription?> GetPrescriptionById(int id)
        {
            return await _context.Prescriptions
                .Include(p => p.Medicine)
                .Include(p => p.Test)
                .FirstOrDefaultAsync(p => p.PrescriptionID == id);
        }

        // Add a new prescription
        public async Task AddPrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        // Optionally, you can add methods to update or delete prescriptions
    }
}
