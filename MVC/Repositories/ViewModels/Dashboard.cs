using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModels
{
    public class Dashboard
    {
        //public List<RequestWiseFile> Files { get; set; }

        public int? RequestId { get; set; }

        public DashboardHeader Header { get; set; }

        //[StringLength(100)]
        //public string FirstName { get; set; } = null!;

        //[StringLength(100)]
        //public string? LastName { get; set; }

        [StringLength(20)]
        public string? StrMonth { get; set; }

        public int? IntYear { get; set; }

        public int? IntDate { get; set; }

        public short? Status { get; set; }

        public int Document { get; set; }
    }
}
