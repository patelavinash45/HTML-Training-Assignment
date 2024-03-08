namespace Services.ViewModels.Admin
{
    public class AssignAndTransferPopUp
    {
        public int SelectedRegion { get; set; }

        public int SelectedPhysician { get; set; }

        public Dictionary<int,String>? Regions { get; set; }

        public Dictionary<int, Tuple<int,String>>? Physicians { get; set; }

        public int RequestId { get; set; }

        public String AdminTransferNotes { get; set; }
    }
}
