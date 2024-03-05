using System.ComponentModel.DataAnnotations;

namespace Repositories.ViewModels.Admin
{
    public class TablesData
    {
        [StringLength(100)]
        public string FirstName { get; set; } 

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(100)]
        public string? PhysicianName { get; set; }

        public int RequesterType { get; set; }

        //public int TotalRequestCount { get; set; }

        public int Requester { get; set; }

        public int RequestId { get; set; }

        public int? RegionId { get; set; }

        [StringLength(100)]
        public string RequesterFirstName { get; set; }

        [StringLength(100)]
        public string? RequesterLastName { get; set; }

        public string Email { get; set; } 

        [StringLength(20)]
        public string? Mobile { get; set; }

        [StringLength(20)]
        public string? RequesterMobile { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? DateOfService { get; set; }

        public DateTime? RequestdDate { get; set; }

        [StringLength(100)]
        public string? Street { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(10)]
        public string? ZipCode { get; set; }
    }
}
