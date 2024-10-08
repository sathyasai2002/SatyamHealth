﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Numerics;

namespace SatyamHealthCare.Models
{
    public class Specialization
    {
        [Key]
        public int SpecializationID { get; set; }

        [Required]
        [MaxLength(255)]
        public string SpecializationName { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<Doctor>? Doctors { get; set; } = new List<Doctor>();
    }
}
