using SatyamHealthCare.Constants;
using SatyamHealthCare.Models;

namespace SatyamHealthCare.IRepos
{
    public interface ILogin
    {
        Task<string> AuthenticateAsync(LoginCred loginCred, Role.UserType userType);
    }
}