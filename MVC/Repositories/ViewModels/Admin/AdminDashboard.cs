using Repositories.DataModels;

namespace Repositories.ViewModels.Admin
{
    public class AdminDashboard
    {
        public DashboardHeader Header { get; set; }

        public List<NewTables> NewRequests { get; set; }

        public int NewRequestCount { get; set; }

        public List<NewTables> PendingRequests { get; set; }

        public int PendingRequestCount { get; set; }

        public List<NewTables> ActiveRequests { get; set; }

        public int ActiveRequestCount { get; set; }

        public List<NewTables> ConcludeRequests { get; set; }

        public int ConcludeRequestCount { get; set; }

        public List<NewTables> TocloseRequests { get; set; }

        public int TocloseRequestCount { get; set; }

        public List<NewTables> UnpaidRequests { get; set; }

        public int UnpaidRequestCount { get; set; }

        public List<String> Regions { get; set; }

        public CancelPopUp CancelPopup { get; set; }
    }
}
