using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTOs
{
    public class PrescriptionDTO
    {
        public int PrescriptionID { get; set; }
        public int NoOfDays { get; set; }
        public string Dosage { get; set; }
        public string BeforeAfterFood { get; set; }
        public string Remark { get; set; }

        public List<int> MedicineIDs { get; set; } = new List<int>();
        public List<int> TestIDs { get; set; } = new List<int>();

        public int AppointmentId { get; set; }

        public string PatientName { get; set; }
        public string DoctorName { get; set; }


    }

}
