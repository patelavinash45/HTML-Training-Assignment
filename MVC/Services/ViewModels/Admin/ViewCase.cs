using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class ViewCase
    {
        public int RequestId { get; set; }

        [StringLength(100)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        [Required]
        public string? LastName { get; set; }

        [StringLength(50)]
        public string? Email { get; set; } = null!;

        [StringLength(20)]
        [Required]
        public string? Mobile { get; set; }

        [StringLength(100)]
        public string? Region { get; set; }

        [StringLength(100)]
        public string? Requester { get; set; }

        [StringLength(100)]
        public string? Room { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(300)]
        public string? PatientNotes { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        public int? AspNetUserId { get; set; }

        public CancelPopUp CancelPopup { get; set; }

    }
}
