using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloDoc.ViewModels
{
    public class AddPatientRequest
    {
        public int UserId { get; set; }

        public int? AspNetUserId { get; set; }

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

        public BitArray? IsMobile { get; set; }

        [StringLength(100)]
        [Required]
        public string? Street { get; set; }

        [StringLength(100)]
        [Required]
        public string? City { get; set; }

        [StringLength(100)]
        [Required]
        public string? State { get; set; }

        public int? RegionId { get; set; }

        [StringLength(10)]
        [Required]
        public string? ZipCode { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        public int CreatedBy { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? ModifiedDate { get; set; }

        public short? Status { get; set; }

        [Column(TypeName = "bit(1)")]
        public BitArray? IsDeleted { get; set; }

        [StringLength(20)]
        public string? Ip { get; set; }
 
        [StringLength(100)]
        public string? House { get; set; }

        public BitArray? IsRequestWithEmail { get; set; }

        [StringLength(500)]
        public string? Symptoms { get; set; }
    }
}
