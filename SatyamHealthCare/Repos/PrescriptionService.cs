using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Repos
{
    public class PrescriptionService : IPrescription
    {
        private readonly SatyamDbContext _context; 
        public PrescriptionService(SatyamDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Prescription>> GetAllPrescriptionsAsync()
        {
            return await _context.Prescriptions.ToListAsync();
        }
        public async Task<Prescription?> GetPrescriptionById(int id, int patientId)
        {
            // Ensure that the prescription belongs to the logged-in patient
            return await _context.Prescriptions
                .Include(p => p.MedicalRecord)
                .FirstOrDefaultAsync(p => p.PrescriptionID == id && p.MedicalRecord.PatientID == patientId);
        }


        public async Task<Prescription> GetPrescriptionByIdAsync(int id)
        {
            return await _context.Prescriptions.FindAsync(id);
        }
        public async Task AddPrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePrescriptionAsync(Prescription prescription)
        {
            _context.Entry(prescription).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeletePrescriptionAsync(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
                await _context.SaveChangesAsync();
            }
        }
    }
}
