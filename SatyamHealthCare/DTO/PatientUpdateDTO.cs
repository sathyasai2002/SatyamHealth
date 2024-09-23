using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace SatyamHealthCare.DTO
{
    public class PatientUpdateDTO
    {
        public int PatientID { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(255)]
        public string BloodGroup { get; set; }

        [Required]
        [MaxLength(15)]
        public string ContactNumber { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string Pincode { get; set; }

        [MaxLength(255)]
        public string City { get; set; }
        [MaxLength(255)]
        public string State { get; set; }
    }
}
