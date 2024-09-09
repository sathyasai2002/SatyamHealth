﻿using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.Models
{
    public class Test
    {
        [Key]
        public int TestID { get; set; }

        [Required]
        [MaxLength(255)]
        public string TestName { get; set; }

        // Navigation property
        public ICollection<PrescribedTest>? PrescribedTests { get; set; } = new List<PrescribedTest>();  
    }
}
