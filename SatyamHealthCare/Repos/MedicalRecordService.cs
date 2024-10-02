using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using Microsoft.EntityFrameworkCore;

namespace SatyamHealthCare.Repos
{
    public class MedicalRecordService : IMedicalRecord
    {
        private readonly SatyamDbContext _context;

        public MedicalRecordService(SatyamDbContext context)
        {
            _context = context;
        }

        // Get all medical records
        public async Task<IEnumerable<MedicalRecord>> GetAllMedicalRecordsAsync()
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Patient) // Eagerly load the related Patient
                .Include(mr => mr.Doctor)  // Eagerly load the related Doctor
                .Include(mr => mr.MedicalHistoryFile) // Eagerly load the related MedicalHistoryFile
                .ToListAsync();
        }

        // Get a medical record by ID
        public async Task<MedicalRecord?> GetMedicalRecordByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(mr => mr.Patient) // Eagerly load the related Patient
                .Include(mr => mr.Doctor)  // Eagerly load the related Doctor
                .Include(mr => mr.MedicalHistoryFile) // Eagerly load the related MedicalHistoryFile
                .FirstOrDefaultAsync(mr => mr.RecordID == id);
        }

        // Add a new medical record
        public async Task<MedicalRecord> AddMedicalRecordAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }

        // Update an existing medical record
        public async Task UpdateMedicalRecordAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Update(medicalRecord);
            await _context.SaveChangesAsync();
        }

        // Delete a medical record
        public async Task DeleteMedicalRecordAsync(int id)
        {
            var medicalRecord = await _context.MedicalRecords.FindAsync(id);
            if (medicalRecord != null)
            {
                _context.MedicalRecords.Remove(medicalRecord);
                await _context.SaveChangesAsync();
            }
        }
    }
}
