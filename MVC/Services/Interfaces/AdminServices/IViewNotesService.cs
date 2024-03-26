using Microsoft.AspNetCore.Http;
using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IViewNotesService
    {
        ViewNotes GetNotes(int RequestId);

        Task<bool> addAdminNotes(String adminNotes, int requestId);

        Task<bool> cancleRequest(CancelPopUp model);

        Task<bool> assignRequest(AssignAndTransferPopUp model);

        Task<bool> blockRequest(BlockPopUp model);

        Task<bool> clearRequest(int requestId);

        bool sendAgreement(Agreement model,HttpContext httpContext);

        Task<bool> agreementDeclined(Agreement model);

        Task<bool> agreementAgree(Agreement model);
    }
}
