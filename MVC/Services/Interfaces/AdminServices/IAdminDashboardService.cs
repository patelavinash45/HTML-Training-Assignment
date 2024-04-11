using Microsoft.AspNetCore.Http;
using Services.ViewModels.Admin;
using System.Data;

namespace Services.Interfaces.AdminServices
{
    public interface IAdminDashboardService
    {
        AdminDashboard getallRequests(int aspNetUserId);

        TableModel GetNewRequest(String status, int pageNo, String patientName, int regionId, int requesterTypeId);

        Dictionary<int, String> getPhysiciansByRegion(int regionId);

        Tuple<String, String, int> getRequestClientEmailAndMobile(int requestId);

        Agreement getUserDetails(String token);

        DataTable exportAllData();

        DataTable exportData(String status, int pageNo, String patientName, int regionId, int requesterTypeId);

        bool SendRequestLink(SendLink model,HttpContext httpContext);

        Task<bool> createRequest(CreateRequest model, int aspNetUserId);

        bool RequestSupport(RequestSupport model);

        EncounterForm getEncounterDetails(int requestId, bool type);

        Task<bool> updateEncounter(EncounterForm model, int requestId);

        ViewCase getRequestDetails(int requestId);

        Task<bool> updateRequest(ViewCase model);
    }
}
