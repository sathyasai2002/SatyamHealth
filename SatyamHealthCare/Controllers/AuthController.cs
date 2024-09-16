using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SatyamHealthCare.Constants;
using SatyamHealthCare.Exceptions;
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return Unauthorized("Invalid Credentials");
                }


                Role.UserType userType = DetermineUserType(loginCred.Email);

                var token = await _loginService.AuthenticateAsync(loginCred, userType);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Invalid credentials");
                }

                return Ok(new { Token = token });
            }
            catch (AuthenticationFailedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return Unauthorized("An error occurred during authentication.");
            }
        }


        private Role.UserType DetermineUserType(string email)
        {
            if (email.Contains("doctor"))
                return Role.UserType.Doctor;
            if (email.Contains("admin"))
                return Role.UserType.Admin;
            return Role.UserType.Patient;
        }

    }
}
    

