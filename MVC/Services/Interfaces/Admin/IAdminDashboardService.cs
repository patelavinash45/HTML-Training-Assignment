using Repositories.ViewModels.Admin;

namespace Services.Interfaces.Admin
{
    public interface IAdminDashboardService
    {
        AdminDashboard getallRequests(int aspNetUserId);

        List<NewTables> GetNewRequest(String status);
    }
}
