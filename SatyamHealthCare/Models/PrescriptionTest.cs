namespace SatyamHealthCare.Models
{
    public class PrescriptionTest
    {
        public int PrescriptionID { get; set; }
        public virtual Prescription Prescription { get; set; }

        public int TestID { get; set; }
        public virtual Test Test { get; set; }
    }
}
