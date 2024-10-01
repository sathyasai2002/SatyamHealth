using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using SatyamHealthCare.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace SatyamHealthCare.Repos
{
    public class PrescriptionService : IPrescription
    {
        private readonly SatyamDbContext _context;
        private readonly IConverter _converter;

        public PrescriptionService(SatyamDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync()
        {
            return await _context.Prescriptions
                .Include(p => p.PrescriptionMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .Include(p => p.PrescriptionTests)
                    .ThenInclude(pt => pt.Test)
                .ToListAsync();
        }

        public async Task<Prescription?> GetPrescriptionById(int id)
        {
            return await _context.Prescriptions
                .Include(p => p.PrescriptionMedicines)
                    .ThenInclude(pm => pm.Medicine)
                .Include(p => p.PrescriptionTests)
                    .ThenInclude(pt => pt.Test)
                .Include(p => p.Appointment)  
                    .ThenInclude(a => a.Patient) 
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Doctor)  
                .FirstOrDefaultAsync(p => p.PrescriptionID == id);
        }
      
        public async Task AddPrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }
    }
}
