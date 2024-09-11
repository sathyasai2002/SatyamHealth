using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTO
{
    public class MedicalRecordDTO
    {
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

        // Optional properties to include related entities
        public PatientDTO? Patient { get; set; }
        public DoctorDTO? Doctor { get; set; }
        public PrescriptionDTO? Prescription { get; set; }

        public ICollection<PrescribedTestDTO>? PrescribedTests { get; set; } = new List<PrescribedTestDTO>();
        public ICollection<PrescriptionDTO>? Prescriptions { get; set; } = new List<PrescriptionDTO>();
    }

  
    
}
