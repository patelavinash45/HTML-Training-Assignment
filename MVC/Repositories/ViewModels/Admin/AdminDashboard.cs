using Repositories.DataModels;

namespace Repositories.ViewModels.Admin
{
    public class AdminDashboard
    {
        public DashboardHeader Header { get; set; }

        public TableModel NewRequests { get; set; }

        public int NewRequestCount { get; set; }

        public TableModel PendingRequests { get; set; }

        public int PendingRequestCount { get; set; }

        public TableModel ActiveRequests { get; set; }

        public int ActiveRequestCount { get; set; }

        public TableModel ConcludeRequests { get; set; }

        public int ConcludeRequestCount { get; set; }

        public TableModel TocloseRequests { get; set; }

        public int TocloseRequestCount { get; set; }

        public TableModel UnpaidRequests { get; set; }

        public int UnpaidRequestCount { get; set; }

        public List<Region> Regions { get; set; }

        public CancelPopUp CancelPopup { get; set; }

        public AssignPopUp AssignPopup { get; set; }

        public BlockPopUp BlockPopup { get; set; }
    }
}
