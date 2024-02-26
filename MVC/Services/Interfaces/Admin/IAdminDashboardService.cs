using Repositories.ViewModels.Admin;

namespace Services.Interfaces.Admin
{
    public interface IAdminDashboardService
    {
        AdminDashboard getallRequests();

        List<NewTables> GetNewRequest(String status);
    }
}
