using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class ViewNotes
    {
        public int RequestId { get; set; }

        public string? PhysicianNotes { get; set; }

        public string? AdminNotes { get; set; }

        [Required]
        public string NewAdminNotes { get; set; }

        public List<String>? TransferNotes { get; set; }
    }
}
