using SatyamHealthCare.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Prescription
{
    [Key]
    public int PrescriptionID { get; set; }

    [Required]
    public int NoOfDays { get; set; }

    [Required]
    [MaxLength(50)]
    public string Dosage { get; set; }

    [Required]
    [MaxLength(100)]
    public string BeforeAfterFood { get; set; }

    [MaxLength(255)]
    public string Remark { get; set; }

    public int? AppointmentId { get; set; }

    [ForeignKey("AppointmentId")]
    public virtual Appointment Appointment { get; set; }


    public virtual ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; }
    public virtual ICollection<PrescriptionTest> PrescriptionTests { get; set; }
}
