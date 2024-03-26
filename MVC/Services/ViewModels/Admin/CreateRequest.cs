using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class CreateRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Street { get; set; }

        public string? Password { get; set; }

        public string? ConformPassword { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string? Room { get; set; }

        public string? Symptoms { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }
    }
}
