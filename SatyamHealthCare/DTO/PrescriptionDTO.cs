using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTOs
{
    public class PrescriptionDTO
    {
        public int PrescriptionID { get; set; }

        public string MedicineName { get; set; }

        public int NoOfDays { get; set; }

        public string Dosage { get; set; }

        public string BeforeAfterFood { get; set; }

        public string Remark { get; set; }


       
        public int MedicineID { get; set; }

        public int TestID { get; set; } 

        public string TestName { get; set; }

    }
}
