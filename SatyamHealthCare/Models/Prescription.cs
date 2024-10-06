using SatyamHealthCare.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Prescription
{
    [Key]
    public int PrescriptionID { get; set; }

    [MaxLength(255)]
    public string Remark { get; set; }

    public int AppointmentId { get; set; }

    [ForeignKey("AppointmentId")]
    public virtual Appointment Appointment { get; set; }

    // Updated navigation property for Prescription Medicines
    public virtual ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; } = new List<PrescriptionMedicine>();

    public virtual ICollection<PrescriptionTest> PrescriptionTests { get; set; }
}