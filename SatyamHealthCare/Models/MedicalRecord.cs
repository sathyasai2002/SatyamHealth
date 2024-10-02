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

        [MaxLength(500)]
        public string Diagnosis { get; set; }

        [Required]
        public int PrescriptionID { get; set; }

        public int MedicalHistoryId { get; set; }

        public MedicalHistoryFile? MedicalHistoryFile { get; set; }

        public Patient? Patient { get; set; }

       
        public Doctor? Doctor { get; set; }

 
       // public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
    }
}
