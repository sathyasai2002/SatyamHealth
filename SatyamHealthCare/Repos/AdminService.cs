using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.Repos
{
    public class AdminService : IAdmin
    {
        private readonly SatyamDbContext _context;

        public AdminService(SatyamDbContext context)
        {
            _context = context;
        }

        // Get all Admins
        public async Task<List<Admin>> GetAllAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        // Get an Admin by ID
        public async Task<Admin?> GetAdminById(int id)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.AdminId == id);
        }

        // Add a new Admin
        public async Task AddAdmin(Admin admin)
        {
         await  _context.Admins.AddAsync(admin);
            
           
        }

        // Update an existing Admin
        public async Task<Admin> UpdateAdmin(Admin admin)
        {
            _context.Admins.Update(admin);
            return admin;
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return false;
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}

