using Microsoft.EntityFrameworkCore;
using SatyamHealthCare.Exceptions;
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
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminId == id);
            if (admin == null)
            {
                throw new EntityNotFoundException("admin", id);
            }
            return admin;

        }

        // Add a new Admin
        public async Task AddAdmin(Admin admin)
        {
            try
            {
                await _context.Admins.AddAsync(admin);
            }

            catch (Exception ex)
            {

                throw new EntityAddFailedException("admin", ex);

            }


        }

        // Update an existing Admin
        public async Task<Admin> UpdateAdmin(Admin admin)
        {
            var existingAdmin = await GetAdminById(admin.AdminId);
            if (existingAdmin == null)
            {
                throw new EntityNotFoundException("Admin", admin.AdminId);
            }
            try
            {
                _context.Admins.Update(admin);
                return admin;
            }
            catch (Exception ex)
            {
                throw new EntityUpdateFailedException("Admin", admin.AdminId, ex);
            }
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
                throw new EntityNotFoundException("Admin", id);
            }
            try
            {
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();

                return true;


            }
            catch (Exception ex)
            {
                throw new EntityDeleteFailedException("Admin", id, ex);
            }


        }
    }

}


