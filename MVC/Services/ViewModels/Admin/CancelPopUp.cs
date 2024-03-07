namespace Services.ViewModels.Admin
{
    public class CancelPopUp
    {
        public int Reason { get; set; }

        public Dictionary<int ,String>? Reasons { get; set; }

        public string? PatientName { get; set; }

        public int RequestId { get; set; }

        public String AdminTransferNotes { get; set; }
    }
}
