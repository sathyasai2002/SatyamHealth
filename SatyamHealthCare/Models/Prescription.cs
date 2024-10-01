using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SatyamHealthCare.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionID { get; set; }

        [Required]
        public int NoOfDays { get; set; }

        [Required]
        [MaxLength(50)]
        public string Dosage { get; set; }

        [Required]
        [MaxLength(10)]
        public string BeforeAfterFood { get; set; }

        [MaxLength(255)]
        public string Remark { get; set; }

        public int MedicineID { get; set; }

        [ForeignKey(nameof(MedicineID))]
        public virtual Medicine? Medicine { get; set; }
      

        public int TestID { get; set; }

        [ForeignKey(nameof(TestID))]
        public virtual Test? Test { get; set; }
    }
}