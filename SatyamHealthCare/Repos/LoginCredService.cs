using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SatyamHealthCare.Constants;
using SatyamHealthCare.Exceptions;
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
            try
            {
                object user = null;
                int? doctorId = null;
                int? patientId = null;

                if (userType == UserType.Patient)
                {
                    var patient = await _context.Patients.SingleOrDefaultAsync(p => p.Email == loginCred.Email && p.Password == loginCred.Password);
                    user = patient;
                    if(patient!= null)
                    {
                        patientId = patient.PatientID;
                    }
                }
                else if (userType == UserType.Doctor)
                {
                    var doctor = await _context.Doctors.SingleOrDefaultAsync(d => d.Email == loginCred.Email && d.Password == loginCred.Password);
                    user = doctor;
                    if (doctor != null)
                    {
                        doctorId = doctor.DoctorId; 
                    }
                }
                else if (userType == UserType.Admin)
                {
                    user = await _context.Admins.SingleOrDefaultAsync(a => a.Email == loginCred.Email && a.Password == loginCred.Password);
                }

                if (user == null)
                {
                    throw new AuthenticationFailedException("Invalid login credentials.");
                }

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginCred.Email),
                    new Claim(ClaimTypes.Role, userType.ToString())

                };

                if (doctorId.HasValue)
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, doctorId.Value.ToString()));
                }
                if (patientId.HasValue)
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, patientId.Value.ToString()));
                    claims.Add(new Claim("PatientId", patientId.Value.ToString()));
                    

                }
                    var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(10),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw new AuthenticationFailedException("An error occurred during authentication.", ex);
            }
        }
    }
}
