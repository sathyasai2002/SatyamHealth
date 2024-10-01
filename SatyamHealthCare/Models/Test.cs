using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.Models
{
    public class Test
    {
        [Key]
        public int TestID { get; set; }

        [Required]
        [MaxLength(255)]
        public string TestName { get; set; }

        public virtual ICollection<PrescriptionTest> PrescriptionTests { get; set; }

    }
}
