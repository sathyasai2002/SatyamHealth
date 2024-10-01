using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTO
{
    public class RescheduleAppointmentDTO
    {
        public int AppointmentId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? NewAppointmentDate { get; set; }


        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? NewAppointmentTime { get; set; }
    }
}
