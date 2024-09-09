using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Repos
{
    public class PatientService:IPatient
    {
        private readonly SatyamDbContext _context;

        public PatientService(SatyamDbContext context)
        {
            _context = context;
        }

        // Get all Patients
        public async Task<List<Patient>> GetAllPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        // Get a Patient by ID
        public async Task<Patient?> GetPatientById(int id)
        {
            return await _context.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.PatientID == id);
        }

        // Add a new Patient
        public async Task<Patient> AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        // Update an existing Patient
        public async Task<Patient> UpdatePatient(Patient patient)
        {
            // Detach existing patient if already being tracked
            var trackedEntity = _context.Patients.Local.FirstOrDefault(p => p.PatientID == patient.PatientID);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            // Proceed with updating the patient
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        // Delete a Patient by ID
        public async Task<bool> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return false;
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
