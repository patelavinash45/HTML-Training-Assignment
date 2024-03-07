using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IAdminDashboardService
    {
        AdminDashboard getallRequests(int aspNetUserId);

        TableModel GetNewRequest(String status, int pageNo);

        TableModel searchPatient(String patientName);
    }
}
