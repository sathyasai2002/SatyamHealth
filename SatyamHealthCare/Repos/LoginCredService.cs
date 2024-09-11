using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SatyamHealthCare.Constants;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using static SatyamHealthCare.Constants.Role;

namespace SatyamHealthCare.Repos
{
    public class LoginCredService : ILogin
    {
        private readonly SatyamDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginCredService(SatyamDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> AuthenticateAsync(LoginCred loginCred, Role.UserType userType)
        {
            // Validate credentials based on userType
            //object user = userType switch
              object user = null;

            if (userType == UserType.Patient)
            {
                user = await _context.Patients.SingleOrDefaultAsync(p => p.Email == loginCred.Email && p.Password == loginCred.Password);
            }
             if (userType == UserType.Doctor)
            {
                user = await _context.Doctors.SingleOrDefaultAsync(d => d.Email == loginCred.Email && d.Password == loginCred.Password);
            }
            else if (userType == UserType.Admin)
            {
                user = await _context.Admins.SingleOrDefaultAsync(a => a.Email == loginCred.Email && a.Password == loginCred.Password);
            }
            if (user == null)
            {
                return null;
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
          Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, loginCred.Email),
            new Claim(ClaimTypes.Role, userType.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);  // Ensure this is not null
        }
    }
}
