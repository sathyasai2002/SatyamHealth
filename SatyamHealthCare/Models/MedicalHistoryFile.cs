using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SatyamHealthCare.Models
{
    public class MedicalHistoryFile
    {
        [Key]
        public int MedicalHistoryId { get; set; }

        [Required]
        
        public int PatientId { get; set; }
        
        public virtual Patient? Patient { get; set; }

        // Yes/No questions
        public bool HasChronicConditions { get; set; }
        public string ChronicConditions { get; set; }

        public bool HasAllergies { get; set; }
        public string Allergies { get; set; }

        public bool TakesMedications { get; set; }
        public string Medications { get; set; }

        public bool HadSurgeries { get; set; }
        public string Surgeries { get; set; }

        public bool HasFamilyHistory { get; set; }
        public string FamilyHistory { get; set; }

        public bool HasLifestyleFactors { get; set; }
        public string LifestyleFactors { get; set; }

        public string VaccinationRecords { get; set; }

        public string CurrentSymptoms { get; set; }
    }
}
