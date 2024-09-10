using System.ComponentModel.DataAnnotations;
using static SatyamHealthCare.Constants.Enum;

namespace SatyamHealthCare.DTO
{
    public class UpdateAppointmentDTO
    {
        public int AppointmentId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    }
}
