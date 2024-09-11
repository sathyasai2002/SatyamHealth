using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.Models
{
    public class LoginCred
    {
        [Required]
        public string Email { get; set; }
      
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain Uppercase, alphanumeric and special characters")]
        public string Password { get; set; }
    
    }
}
