using Repositories.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IViewCaseService
    {
        ViewCase getRequestDetails(int requestId);

        Task<bool> updateRequest(ViewCase model);

    }
}
