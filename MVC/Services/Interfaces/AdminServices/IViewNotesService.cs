using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IViewNotesService
    {
        ViewNotes GetNotes(int RequestId);

        Task<bool> addAdminNotes(ViewNotes model);

        Task<bool> cancleRequest(CancelPopUp model);

        Task<bool> assignRequest(AssignAndTransferPopUp model);

        Task<bool> blockRequest(BlockPopUp model);

        Task<bool> clearRequest(int requestId);
    }
}
