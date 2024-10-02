using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SatyamHealthCare.Models
{
    public class MedicalHistoryFile
    {
        [Key]
        public int MedicalHistoryId { get; set; }

        [Required]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

        public bool HasChronicConditions { get; set; } = false;
        [MaxLength(255)]
        public string? ChronicConditions { get; set; }

        public bool HasAllergies { get; set; } = false;
        [MaxLength(255)]
        public string? Allergies { get; set; }

        public bool TakesMedications { get; set; } = false;
        [MaxLength(255)]
        public string? Medications { get; set; }

        public bool HadSurgeries { get; set; } = false;
        [MaxLength(255)]
        public string? Surgeries { get; set; }

        public bool HasFamilyHistory { get; set; } = false;
        [MaxLength(255)]
        public string? FamilyHistory { get; set; }

        public bool HasLifestyleFactors { get; set; } = false;
        [MaxLength(255)]
        public string? LifestyleFactors { get; set; }

        [MaxLength(255)]
        public string? VaccinationRecords { get; set; }

        public ICollection<MedicalRecord>? MedicalRecords { get; set; }


    }
}
