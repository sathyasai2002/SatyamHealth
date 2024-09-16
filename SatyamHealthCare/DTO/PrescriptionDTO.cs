using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTO
{
    public class PrescriptionDTO
    {
        [Required]
        public int PrescriptionID { get; set; }

        [Required]
        [MaxLength(255)]
        public string MedicineName { get; set; }

        [Required]
        public int NoOfDays { get; set; }

        [Required]
        [MaxLength(50)]
        public string Dosage { get; set; }

        [Required]
        [MaxLength(10)]
        public string BeforeAfterFood { get; set; }

      public MedicalRecordDTO? MedicalRecord { get; set; }
    }
}
