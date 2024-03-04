using Repositories.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IAdminDashboardService
    {
        AdminDashboard getallRequests(int aspNetUserId);

        List<NewTables> GetNewRequest(String status);
    }
}
