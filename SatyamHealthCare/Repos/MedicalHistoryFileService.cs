
   
    using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
    using SatyamHealthCare.Models;
    namespace SatyamHealthCare.Repos
    {
        public class MedicalHistoryFileService : IMedicalHistoryFile
        {
            private readonly SatyamDbContext _context;
            public MedicalHistoryFileService(SatyamDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<MedicalHistoryFile>> GetAllMedicalHistoryFiles()
            {
                return await _context.MedicalHistoryFiles
                .Include(m => m.Patient)
                .ToListAsync();
            }

        public async Task<MedicalHistoryFile> GetMedicalHistoryByPatientIdAsync(int patientId)
        {
            return await _context.MedicalHistoryFiles
                .FirstOrDefaultAsync(m => m.PatientId == patientId);
        }
        public async Task<MedicalHistoryFile> GetMedicalHistoryFileById(int id)
            {
                return await _context.MedicalHistoryFiles
                .Include(m => m.Patient)
                .FirstOrDefaultAsync(m => m.MedicalHistoryId == id);
            }
           /* public async Task<IEnumerable<MedicalHistoryFile>> GetMedicalHistoryFilesByPatientId(int
           patientId)
            {
                return await _context.MedicalHistoryFiles
                .Where(m => m.PatientId == patientId)
                .ToListAsync();
            }*/
            public async Task AddMedicalHistoryFile(MedicalHistoryFile medicalHistoryFile)
            {
                _context.MedicalHistoryFiles.Add(medicalHistoryFile);
                await _context.SaveChangesAsync();
            }
            public async Task UpdateMedicalHistoryFile(MedicalHistoryFile medicalHistoryFile)
            {
                _context.Entry(medicalHistoryFile).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            public async Task DeleteMedicalHistoryFile(int id)
            {
                var medicalHistoryFile = await _context.MedicalHistoryFiles.FindAsync(id);
                if (medicalHistoryFile != null)
                {
                    _context.MedicalHistoryFiles.Remove(medicalHistoryFile);
                    await _context.SaveChangesAsync();
                }
            }
            public async Task<bool> MedicalHistoryFileExists(int id)
            {
                return await _context.MedicalHistoryFiles.AnyAsync(m => m.MedicalHistoryId == id);
            }
            public MedicalHistoryFile GetById(int id)
            {
                return _context.MedicalHistoryFiles.FirstOrDefault(m => m.MedicalHistoryId == id);
            }


    }
    }
