using Microsoft.AspNetCore.Mvc;
using SatyamHealthCare.Models;

        namespace SatyamHealthCare.IRepos
        {
            public interface IAdmin
            {
                Task<List<Admin>> GetAllAdmins();
                Task<Admin?> GetAdminById(int id);
                  Task AddAdmin(Admin admin);
                Task<Admin> UpdateAdmin(Admin admin);
        Task Save();
                Task<bool> DeleteAdmin(int id);
    }
        }
