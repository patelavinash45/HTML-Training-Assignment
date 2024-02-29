using Repositories.DataModels;

namespace Repositories.ViewModels.Admin
{
    public class AssignPopUp
    {
        public int SelectedRegion { get; set; }

        public int SelectedPhysician { get; set; }

        public List<Region>? Regions { get; set; }

        public List<Physician>? Physics { get; set; }

        public int RequestId { get; set; }

        public String AdminTransferNotes { get; set; }
    }
}
