using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class ViewProfile
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

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        public int IsMobile { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }
    }
}
