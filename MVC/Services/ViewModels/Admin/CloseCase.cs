using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class CloseCase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        public List<FileModel>? FileList { get; set; }

    }
}
