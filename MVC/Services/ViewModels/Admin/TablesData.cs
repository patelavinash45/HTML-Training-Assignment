namespace Services.ViewModels.Admin
{
    public class TablesData
    {
        public string FirstName { get; set; } 

        public string LastName { get; set; }

        public string Notes { get; set; }

        public string PhysicianName { get; set; }

        public int RequesterType { get; set; }

        public int? AssignPhysician { get; set; }

        public int Requester { get; set; }

        public int RequestId { get; set; }

        public int? RegionId { get; set; }

        public string RequesterFirstName { get; set; }

        public string? RequesterLastName { get; set; }

        public string Email { get; set; } 

        public string? Mobile { get; set; }

        public string? RequesterMobile { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? DateOfService { get; set; }

        public DateTime? RequestdDate { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }
    }
}
