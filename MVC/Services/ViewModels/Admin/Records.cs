using Repositories.DataModels;

namespace Services.ViewModels.Admin
{
    public class Records
    {
        public int? RequestStatus { get; set; }

        public string? PatientName { get; set; }

        public int? RequestType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ProviderName { get; set; }

        public string? Email { get; set;}

        public string? Number { get; set; }

        public List<RecordTableData>? RecordTableDatas { get; set; }
    }

    public class RecordTableData
    {
        public int RequestId { get; set; }

        public string PatientName { get; set; }

        public int RequestType { get; set;}

        public DateTime? DateOfService { get; set; }

        public DateTime ColseCaseDate { get; set;}

        public string Email { get; set;}

        public string Phone { get; set;}

        public string Address { get; set;}

        public string Zip { get; set;}

        public int Status { get; set; }

        public string PhysicianName { get; set; } = "-";

        public string PhysicianNotes { get; set; } = "-";

        public string CancleNotes { get; set; } = "-";

        public string AdminNotes { get; set; } = "-";

        public string PatientNotes { get; set; } = "-";
    }

    public class EmailSmsLogs
    {
        public Dictionary<int , string>? Roles { get; set; }

        public int? Role { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateTime? SendDate { get; set; } 

        public DateTime? CreatedDate { get; set;} 

        public List<EmailSmsLogTableData>? EmailSmsLogTableDatas { get; set; }
    }

    public class EmailSmsLogTableData
    {
        public string Name { get; set; }

        public string Action { get; set; }

        public string RoleName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime CreatedDate { get; set;}

        public string Send { get; set; }


    }
}
