using Services.ViewModels.Admin;
using System.Data;

namespace Services.Interfaces.AdminServices
{
    public interface IAdminDashboardService
    {
        AdminDashboard getallRequests(int aspNetUserId);

        TableModel GetNewRequest(String status, int pageNo);

        TableModel patientSearch(String searchElement, String status, int pageNo, int type);

        Dictionary<int, String> getPhysiciansByRegion(int regionId);

        Tuple<String, String, int> getRequestClientEmailAndMobile(int requestId);

        Agreement getUserDetails(String token);

        DataTable exportAllData();
    }
}
