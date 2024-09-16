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
        [DefaultValue(AppointmentStatus.Pending)]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    }
}
