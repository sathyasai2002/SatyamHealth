using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SatyamHealthCare.Models
{
    public class MedicalRecord
    {
        [Key]
        public int RecordID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int DoctorID { get; set; }

        [Required]
        public DateTime ConsultationDateTime { get; set; }

        [MaxLength(500)]
        public string Diagnosis { get; set; }

        [Required]
        public int PrescriptionID { get; set; }

        // Foreign Keys and Navigation Properties
        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }

        [ForeignKey("DoctorID")]
        public Doctor? Doctor { get; set; }

        [ForeignKey("PrescriptionID")]
        public Prescription? Prescription { get; set; }

        public virtual ICollection<PrescribedTest>? PrescribedTests { get; set; } = new List<PrescribedTest>();
        public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
    }
}
