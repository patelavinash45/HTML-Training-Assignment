using Microsoft.AspNetCore.Http;
using Repositories.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModels
{
    public class ViewDocument
    { 
        public List<RequestWiseFile> FileList { get; set; }

        public DashboardHeader Header { get; set; }

        [Required]
        public IFormFile? File { get; set; }

        public int RequestId { get; set; }

        //[StringLength(500)]
        //public string FileName { get; set; } = null!;

        //[StringLength(100)]
        //public string Uploder { get; set; } = null!;

        //public int Day { get; set; }

        //public int Month { get; set; } 

        //public int Year { get; set; }

        //public int? PhysicianId { get; set; }

        //public int? AdminId { get; set; }
    }
}
