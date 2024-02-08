using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloDoc.ViewModels
{
    public class AddConciergeRequest
    {
        [StringLength(100)]
        [Required]
        public string ConciergeFirstName { get; set; } = null!;

        [StringLength(100)]
        [Required]
        public string? ConciergeLastName { get; set; }

        [StringLength(50)]
        [Required]
        [RegularExpression("^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\\-+)|([A-Za-z0-9]+\\.+)|([A-Za-z0-9]+\\++))*[A-Za-z0-9]+@((\\w+\\-+)|(\\w+\\.))*" +
            "\\w{1,63}\\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email")]
        public string ConciergeEmail { get; set; } = null!;

        [StringLength(20)]
        [Required]
        public string? ConciergeMobile { get; set; }

        [StringLength(100)]
        [Required]
        public string? ConciergePropertyName { get; set; }

        [StringLength(100)]
        [Required]
        public string? ConciergeStreet { get; set; }

        [StringLength(100)]
        [Required]
        public string? ConciergeCity { get; set; }

        [StringLength(100)]
        [Required]
        public string? ConciergeState { get; set; }

        [StringLength(10)]
        [Required]
        public string? ConciergeZipCode { get; set; }

        [StringLength(100)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        [Required]
        public string? LastName { get; set; }

        [StringLength(50)]
        [Required]
        [RegularExpression("^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\\-+)|([A-Za-z0-9]+\\.+)|([A-Za-z0-9]+\\++))*[A-Za-z0-9]+@((\\w+\\-+)|(\\w+\\.))*" +
            "\\w{1,63}\\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = null!;

        [StringLength(20)]
        [Required]
        public string? Mobile { get; set; }

        [StringLength(100)]
        [Required]
        public string? Street { get; set; }

        [StringLength(100)]
        [Required]
        public string? City { get; set; }

        [StringLength(100)]
        [Required]
        public string? State { get; set; }

        [StringLength(10)]
        [Required]
        public string? ZipCode { get; set; }

        [StringLength(500)]
        public string? Symptoms { get; set; }

        [StringLength(100)]
        public string? House { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

    }
}
