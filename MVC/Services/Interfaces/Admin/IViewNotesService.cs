using Repositories.ViewModels.Admin;

namespace Services.Interfaces.Admin
{
    public interface IViewNotesService
    {
        Task<ViewNotes> GetNotes(int RequestId);

        Task<bool> addAdminNotes(ViewNotes model);

        Task<bool> cancleRequest(CancelPopUp model);

        Task<bool> assignRequest(AssignPopUp model);

        Task<bool> blockRequest(BlockPopUp model);
    }
}
