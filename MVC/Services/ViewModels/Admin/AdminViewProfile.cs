using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class AdminViewProfile
    {
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public int SelectedRegions { get; set; }

        public string SelectedRegion { get; set; }

        public Dictionary<int, string>? Regions { get; set; }

        public Dictionary<int, bool>? AdminRegions { get; set; }

        [Required]
        public short? Status { get; set; }

        public string Role { get; set; }

    }
}
