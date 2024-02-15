using System.ComponentModel.DataAnnotations;

namespace HelloDoc.ViewModels
{
    public class DashboardHeader
    {
        [StringLength(100)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        [Required]
        public string? LastName { get; set; }

        public int? userId { get; set; }
    }
}
