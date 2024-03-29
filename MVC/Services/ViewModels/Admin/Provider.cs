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
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "The Role field is required.")]
        public int SelectedRole { get; set; }

        public Dictionary<int, string>? Roles { get; set; }

        public string UserName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(20)]
        public string Phone2 { get; set; }

        [StringLength(500)]
        public string MedicalLicance { get; set; }

        [StringLength(500)]
        public string NpiNumber { get; set; }

        [StringLength(500)]
        public string Add1 { get; set; }

        [StringLength(500)]
        public string Add2 { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(10)]
        public string Zip { get; set; }

        [Required(ErrorMessage = "The State field is required.")]
        public string SelectedRegion { get; set; }

        [Required(ErrorMessage = "The Region field is required.")]
        public List<string> SelectedRegions { get; set; }

        public Dictionary<int,string>? Regions { get; set; }

        [StringLength(100)]
        public string BusinessName { get; set; }

        [StringLength(200)]
        public string BusinessWebsite { get; set; }

        public IFormFile Photo { get; set; }

        [StringLength(500)]
        public string AdminNotes { get; set; }

        public bool IsAgreementDoc { get; set; }

        [FileRequiredIfBoolIsTrue(nameof(IsAgreementDoc))]
        public IFormFile? AgreementDoc { get; set; }

        public bool IsBackgroundDoc { get; set; }

        [FileRequiredIfBoolIsTrue(nameof(IsBackgroundDoc))]
        public IFormFile? BackgroundDoc { get; set; }

        public bool IsHIPAACompliance { get; set; }

        [FileRequiredIfBoolIsTrue(nameof(IsHIPAACompliance))]
        public IFormFile? HIPAACompliance { get; set; }

        public bool IsNonDisclosureDoc { get; set; }

        [FileRequiredIfBoolIsTrue(nameof(IsNonDisclosureDoc))]
        public IFormFile? NonDisclosureDoc { get; set; }

    }

    public class FileRequiredIfBoolIsTrueAttribute : ValidationAttribute
    {
        private readonly string[] _boolProperties;

        public FileRequiredIfBoolIsTrueAttribute(params string[] boolProperties)
        {
            _boolProperties = boolProperties;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (var boolProperty in _boolProperties)
            {
                var property = validationContext.ObjectType.GetProperty(boolProperty);

                if (property == null)
                {
                    return new ValidationResult($"Unknown property {boolProperty}");
                }

                var boolPropertyValue = (bool)property.GetValue(validationContext.ObjectInstance);

                if (boolPropertyValue && value == null)
                {
                    return new ValidationResult($"{validationContext.DisplayName} is required");
                }
            }

            return ValidationResult.Success;
        }
    }

    public class ProviderScheduling
    {
        public Dictionary<int, string> Regions { get; set; }

        public List<SchedulingTable> TableData { get; set; }

    }

    public class SchedulingTable
    {
        public string Photo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

}
