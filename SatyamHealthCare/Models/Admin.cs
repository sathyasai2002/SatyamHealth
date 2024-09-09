using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SatyamHealthCare.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
