using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTO
{
    public class PrescribedTestDTO
    {
        [Required]
        public int PrescribedTestID { get; set; }

        [Required]
        public int RecordID { get; set; }

        [Required]
        public int TestID { get; set; }

        [MaxLength(500)]
        public string TestResult { get; set; }

        public TestDTO? Test { get; set; }
    }

  
}
