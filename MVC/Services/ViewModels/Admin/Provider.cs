using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class Provider
    {
        public List<ProviderTable> providers { get; set; }

        public Dictionary<int, string> Regions { get; set; }

        public ContactProvider ContactProvider { get; set; }
    }

    public class ProviderTable
    {
        public int providerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Notification { get; set; }

        public string Role { get; set; }

        public bool OnCallStatus { get; set; }

        public String Status { get; set; }
    }

    public class ContactProvider
    {
        public int providerId { get; set;}

        public string Message { get; set; }

        public bool email { get; set; } = false;

        public bool sms { get; set; } = false;
    }

    public class CreateProvider
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "The Role field is required.")]
        public int SelectedRole { get; set; }

        public Dictionary<int, string>? Roles { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Phone2 { get; set; }

        public string MedicalLicance { get; set; }

        public string NpiNumber { get; set; }

        public string Add1 { get; set; }

        public string Add2 { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage = "The State field is required.")]
        public string SelecterRegion { get; set; }

        [Required(ErrorMessage = "The Region field is required.")]
        public List<string> SelecterRegions { get; set; }

        public Dictionary<int,string>? Regions { get; set; }

        public string BusinessName { get; set; }

        public string BusinessWebsite { get; set; }

        public IFormFile Photo { get; set; }

        public string AdminNotes { get; set; }

    }
}
