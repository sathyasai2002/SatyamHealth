using SatyamHealthCare.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SatyamHealthCare.Repositories
{
    public interface ISpecialization
    {
        Task<IEnumerable<Specialization>> GetAllSpecializationsAsync();
    }
}