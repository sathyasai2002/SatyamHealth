using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Numerics;
using static SatyamHealthCare.Constants.Status;

namespace SatyamHealthCare.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }
        public virtual Doctor? Doctor { get; set; } 

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? AppointmentDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? AppointmentTime { get; set; }

        [Required]
        [DefaultValue(AppointmentStatus.Pending)]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public string Symptoms { get; set; }

        public virtual ICollection<Prescription> Prescriptions { get; set; }

    }
}
