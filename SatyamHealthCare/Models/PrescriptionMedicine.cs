using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.Models
{
    public class PrescriptionMedicine
    {
        public int PrescriptionID { get; set; }
        public virtual Prescription Prescription { get; set; }

        public int MedicineID { get; set; }
        public virtual Medicine Medicine { get; set; }

        [Required]
        [MaxLength(50)]
        public string Dosage { get; set; } 

        [Required]
        [MaxLength(50)]
        public string DosageFrequency { get; set; } 

        [Required]
        [MaxLength(10)]
        public string DosageUnit { get; set; } 

        [Required]
        public int NoOfDays { get; set; } 

        [Required]
        [MaxLength(50)]
        public string BeforeAfterFood { get; set; }
    }
}
