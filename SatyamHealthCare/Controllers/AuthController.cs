using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SatyamHealthCare.Constants;
using SatyamHealthCare.IRepos;
using SatyamHealthCare.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SatyamHealthCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogin _loginService;
        private readonly IConfiguration _configuration;

        public AuthController(ILogin loginService, IConfiguration configuration)
        {
            _loginService = loginService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCred loginCred)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Determine user type (implement your own logic here)
            Role.UserType userType = DetermineUserType(loginCred.Email);

            var token = await _loginService.AuthenticateAsync(loginCred, userType);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { Token = token });
        }

        private Role.UserType DetermineUserType(string email)
        {
            if (email.Contains("doctor"))
                return Role.UserType.Doctor;
            if (email.Contains("admin"))
                return Role.UserType.Admin;
           return Role.UserType.Patient;
        }
        private string GenerateJwtToken(Role.UserType userType, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(ClaimTypes.Role, userType.ToString()),  // Role claim
                new Claim(JwtRegisteredClaimNames.Jti, System.Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: System.DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
