namespace SatyamHealthCare.Models
{
    public class PrescriptionMedicine
    {
        
    public int PrescriptionID { get; set; }
        public virtual Prescription Prescription { get; set; }

        public int MedicineID { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}

