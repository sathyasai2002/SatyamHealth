using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.Models
{
    public class PrescribedTest
    {
        [Key]
        public int PrescribedTestID { get; set; }
        [Required]
        public int RecordID { get; set; }

        [Required]
        public int TestID { get; set; }

        [MaxLength(500)]
        public string TestResult { get; set; }

        // Foreign Keys and Navigation Properties
        
        public MedicalRecord? MedicalRecord { get; set; }

        
        public Test? Test { get; set; }
    }
}
