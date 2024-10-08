﻿using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTO
{
    public class PatientDTO
    {
        public int PatientID { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(255)]
        public string BloodGroup { get; set; }

        [Required]
        [MaxLength(15)]
        public string ContactNumber { get; set; }

        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string Pincode { get; set; }

        [MaxLength(255)]
        public string City { get; set; }
        [MaxLength(255)]
        public string State { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Password { get; set; }

       // public byte[]? ProfilePicture { get; set; }
    }
}
