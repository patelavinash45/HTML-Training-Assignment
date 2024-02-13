using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelloDoc.ViewModels
{
    public class Dashboard
    {

        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(20)]
        public string? StrMonth { get; set; }

        public int? IntYear { get; set; }

        public int? IntDate { get; set; }

        public short? Status { get; set; }

        public List<int> Document { get; set; }
    }
}
