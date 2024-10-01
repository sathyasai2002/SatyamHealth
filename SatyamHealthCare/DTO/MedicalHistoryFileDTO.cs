using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.DTO
{
    public class MedicalHistoryFileDTO
    {
        public int MedicalHistoryId { get; set; }

        [Required]
        public int PatientId { get; set; }

        public bool HasChronicConditions { get; set; }

        [MaxLength(255)]
        public string? ChronicConditions { get; set; }

        public bool HasAllergies { get; set; }

        [MaxLength(255)]
        public string? Allergies { get; set; }

        public bool TakesMedications { get; set; }

        [MaxLength(255)]
        public string? Medications { get; set; }

        public bool HadSurgeries { get; set; }

        [MaxLength(255)]
        public string? Surgeries { get; set; }

        public bool HasFamilyHistory { get; set; }

        [MaxLength(255)]
        public string? FamilyHistory { get; set; }

        public bool HasLifestyleFactors { get; set; }

        [MaxLength(255)]
        public string? LifestyleFactors { get; set; }

        [MaxLength(255)]
        public string? VaccinationRecords { get; set; }

  
    }
}
