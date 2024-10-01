using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.Models
{
    public class Medicine
    {
        [Key]
        public int MedicineID { get; set; }

        [Required]
        [MaxLength(255)]
        public string MedicineName { get; set; }


        public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
    }
}
