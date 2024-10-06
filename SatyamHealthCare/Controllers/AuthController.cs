using log4net;
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
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AuthController));

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
                _logger.Info($"Login attempt for email: {loginCred.Email}");

                if (!ModelState.IsValid)
                {
                    _logger.Warn($"Invalid model state for login attempt: {loginCred.Email}");
                    return Unauthorized("Invalid Credentials");
                }

                Role.UserType userType = DetermineUserType(loginCred.Email);
                _logger.Info($"Determined user type: {userType} for email: {loginCred.Email}");

                var token = await _loginService.AuthenticateAsync(loginCred, userType);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.Warn($"Authentication failed for email: {loginCred.Email}");
                    return Unauthorized("Invalid credentials");
                }

                string emailInitial = loginCred.Email.Length > 0 ? loginCred.Email[0].ToString().ToUpper() : "";
                _logger.Info($"Successful login for email: {loginCred.Email}");
                return Ok(new { Token = token, EmailInitial = emailInitial });
            }
            catch (AuthenticationFailedException ex)
            {
                _logger.Error($"Authentication failed for email: {loginCred.Email}", ex);
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred during authentication for email: {loginCred.Email}", ex);
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
    

