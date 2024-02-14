using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloDoc.ViewModels
{
    public class ViewDocument
    { 
        [StringLength(500)]
        public string FileName { get; set; } = null!;

        [StringLength(100)]
        public string Uploder { get; set; } = null!;

        public int Day { get; set; }

        public int Month { get; set; } 

        public int Year { get; set; }

        public int? PhysicianId { get; set; }

        public int? AdminId { get; set; }
    }
}
