using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModels
{
    public class DashboardHeader
    {
        [StringLength(100)]
        [Required]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        [Required]
        public string? LastName { get; set; }

        public int? AspNetUserId { get; set; }
    }
}
