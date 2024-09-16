using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTO
{
    public class MedicalRecordDTO
    {


        [Required]
        public int PatientID { get; set; }

        [Required]
        public int DoctorID { get; set; }
        
        public int RecordID { get; set; }
        [Required]
        public DateTime ConsultationDateTime { get; set; }

        [MaxLength(500)]
        public string Diagnosis { get; set; }

        [Required]
        public int PrescriptionID { get; set; }
    }

  
    
}
