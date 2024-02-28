using Repositories.ViewModels.Admin;

namespace Services.Interfaces.Admin
{
    public interface IViewNotesService
    {
        Task<ViewNotes> GetNotes(int RequestId);

        Task<bool> addAdminNotes(ViewNotes model);

        Task<bool> addAdminTransform(CancelPopUp mode);
    }
}
