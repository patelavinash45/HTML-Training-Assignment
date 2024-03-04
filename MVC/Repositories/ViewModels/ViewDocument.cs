using Microsoft.AspNetCore.Http;
using Repositories.DataModels;
using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModels
{
    public class ViewDocument
    { 
        public List<RequestWiseFile>? FileList { get; set; }

        public DashboardHeader Header { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public int RequestId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
