namespace Services.ViewModels.Admin
{
    public class AdminDashboard
    {
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

        public Dictionary<int,String> Regions { get; set; }

        public CancelPopUp CancelPopup { get; set; }

        public AssignAndTransferPopUp AssignAndTransferPopup { get; set; }

        public BlockPopUp BlockPopup { get; set; }
    }
}
