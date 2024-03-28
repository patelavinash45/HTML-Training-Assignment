using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class CreateRequest
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string Street { get; set; }

        public string? Password { get; set; }

        public string? ConformPassword { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        public string? Room { get; set; }

        [StringLength(500)]
        public string? Symptoms { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }
    }
}
