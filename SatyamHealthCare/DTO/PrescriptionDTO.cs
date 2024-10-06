using System.Collections.Generic;

namespace SatyamHealthCare.DTOs
{
    public class PrescriptionDTO
    {
        public int PrescriptionID { get; set; } 
        public string Remark { get; set; } 
        public int AppointmentId { get; set; } 

        public List<MedicineDTO> Medicines { get; set; } = new List<MedicineDTO>();
        public List<int> TestIDs { get; set; } = new List<int>();

        // Add TestNames for GET requests


    }

    public class MedicineDTO
    {
        public int MedicineID { get; set; } 
        public string Dosage { get; set; }

        public string  DosageUnit { get; set; }
        public int NoOfDays { get; set; }

        public string DosageFrequency { get; set; }
        public string BeforeAfterFood { get; set; }
    }

}
