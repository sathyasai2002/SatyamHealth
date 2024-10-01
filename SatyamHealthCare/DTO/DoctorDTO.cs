using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTO
{
    public class DoctorDTO
    {
        public int DoctorId { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string PhoneNo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Designation { get; set; }

        [Required]
        public int Experience { get; set; }

        
        public int SpecializationID { get; set; }

        [Required]
        public string Qualification { get; set; }

        public string? SpecializationName { get; set; }

        [Required]
        public int AdminId { get; set; }


    }
}
