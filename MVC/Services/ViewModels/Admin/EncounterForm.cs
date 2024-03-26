using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class EncounterForm
    {
        public bool IsAdmin { get; set; } = true;
        
        public string FirstName { get; set; }
                        
        public string LastName { get; set; }

        public string Location { get; set; }

        public string Email { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        [Required]
        public DateTime? Date { get; set; }
        
        public string Mobile { get; set; }
        
        public string HistoryOfIllness { get; set; }
        
        public string MedicalHistory { get; set; }
        
        public string Medications { get; set; }
        
        public string Allergies { get; set; }
        
        public string Temp { get; set; }
        
        public string HeartRate { get; set; }
        
        public string RespiratoryRate { get; set; }

        [Required(ErrorMessage = "The BloodPressure field is required.")]
        public string BloodPressure1 { get; set; }

        [Required(ErrorMessage = "The BloodPressure field is required.")]
        public string BloodPressure2 { get; set; }
        
        public string O2 { get; set; }
        
        public string Pain { get; set; }
        
        public string Heent { get; set; }
        
        public string CV { get; set; }
        
        public string Chest { get; set; }
        
        public string ABD { get; set; }
        
        public string Extra { get; set; }
        
        public string Skin { get; set; }
        
        public string Neuro { get; set; }
        
        public string Other { get; set; }
        
        public string Diagnosis { get; set; }
        
        public string TreatmentPlan { get; set; }
        
        public string Dispensed { get; set; }
        
        public string Procedures { get; set; }
        
        public string FollowUp { get; set; }
    }
}
