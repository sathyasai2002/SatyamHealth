using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SatyamHealthCare.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string PhoneNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string Designation { get; set; }

        [Required]
        [StringLength(100)]
        public int Experience { get; set; }

        [Required]
        public int SpecializationID {  get; set; }

        [ForeignKey("SpecializationID")]
        public virtual Specialization? Specialization { get; set; }

        [Required]
        [StringLength (100)]
        public string Qualification { get; set; }

        public byte[] ProfilePicture { get; set; }

        public int AdminId { get; set; }
        public virtual Admin? Admin { get; set; }



        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; } = new List<MedicalRecord>();
    }
}
