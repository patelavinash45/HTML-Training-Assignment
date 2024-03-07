using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class Dashboard
    {

        public int? RequestId { get; set; }

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
