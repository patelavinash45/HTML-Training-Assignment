using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class ViewCase
    {
        public bool IsAdmin { get; set; } = true;

        public int? Status { get; set; }

        public int RequestId { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; } 

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string? Email { get; set; } 

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        public int Requester { get; set; } = 0;

        [StringLength(100)]
        public string? Room { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(300)]
        public string? PatientNotes { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        public CancelPopUp? CancelPopup { get; set; }

    }
}
