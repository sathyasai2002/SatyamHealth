using System.ComponentModel.DataAnnotations;
using static SatyamHealthCare.Constants.Status;

namespace SatyamHealthCare.DTO
{
    public class UpdatestatusDTO
    {

        [Required]
        public AppointmentStatus Status { get; set; }
    }
}
