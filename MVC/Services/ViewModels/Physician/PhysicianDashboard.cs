using Services.ViewModels.Admin;

namespace Services.ViewModels.Physician
{
    public class PhysicianDashboard
    {
        public TableModel NewRequests { get; set; }

        public int NewRequestCount { get; set; }

        public TableModel PendingRequests { get; set; }

        public int PendingRequestCount { get; set; }

        public TableModel ActiveRequests { get; set; }

        public int ActiveRequestCount { get; set; }

        public TableModel ConcludeRequests { get; set; }

        public int ConcludeRequestCount { get; set; }

        public CancelPopUp CancelPopup { get; set; }

        public Agreement SendAgreement { get; set; }

        public AssignAndTransferPopUp AssignAndTransferPopup { get; set; }

        public BlockPopUp BlockPopup { get; set; }

        public SendLink SendLink { get; set; }
    }
}
