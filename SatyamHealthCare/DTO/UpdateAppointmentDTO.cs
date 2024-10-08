﻿using System.ComponentModel.DataAnnotations;
using static SatyamHealthCare.Constants.Status;

namespace SatyamHealthCare.DTO
{
    public class UpdateAppointmentDTO
    {
        public int AppointmentId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? AppointmentDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        public TimeSpan? AppointmentTime { get; set; }
        public string? PatientName { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    }
}
