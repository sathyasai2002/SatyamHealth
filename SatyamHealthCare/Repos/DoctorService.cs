using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Repos
{
    public class DoctorService:IDoctor

    {

        private readonly SatyamDbContext _context;
        public DoctorService(SatyamDbContext context)
        {
            _context = context;
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<Doctor>> GetAllDoctors()
        {
            return await _context.Doctors
                .Include(d => d.Specialization)
                .Select(d => new Doctor
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Designation = d.Designation,
                    Experience = d.Experience,
                    Specialization = d.Specialization
                })
                .ToListAsync();
        }
        public async Task<Doctor> AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

        public async Task<Doctor?> GetDoctorById(int id)
        {
            return await _context.Doctors
                  .Include(d => d.Admin)
                .Include(d => d.Specialization)
                .Where(d => d.DoctorId == id)
                .Select(d => new Doctor
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Designation = d.Designation,
                    Experience = d.Experience,
                    Specialization = d.Specialization,
                    Email = d.Email,
                    Admin = d.Admin
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdateDoctor(Doctor doctor)
        {
            _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
           
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

        }
    }

    }



