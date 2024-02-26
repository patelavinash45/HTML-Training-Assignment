using Repositories.ViewModels.Admin;

namespace Services.Interfaces.Admin
{
    public interface IViewCaseService
    {
        ViewCase getRequestDetails(int requestId);

        Task<bool> updateRequest(ViewCase model);

        Task<bool> cancelRequest(int requestId);
    }
}
