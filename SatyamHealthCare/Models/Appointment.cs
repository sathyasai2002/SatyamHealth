using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Numerics;
using static SatyamHealthCare.Constants.Enum;

namespace SatyamHealthCare.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [DefaultValue(AppointmentStatus.Pending)]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    }
}
