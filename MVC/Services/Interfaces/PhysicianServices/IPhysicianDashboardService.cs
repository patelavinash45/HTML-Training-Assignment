using Services.ViewModels.Admin;
using Services.ViewModels.Physician;

namespace Services.Interfaces.PhysicianServices
{
    public interface IPhysicianDashboardService
    {
        PhysicianDashboard getallRequests(int aspNetUserId);

        TableModel GetNewRequest(String status, int pageNo, String patientName, int regionId, int requesterTypeId, int aspNetUserId);

        Task<bool> acceptRequest(int requestId);

        Task<bool> transferRequest(PhysicianTransferRequest model);

        Task<bool> setEncounter(int requestId,bool isVideoCall);

        int getPhysicianIdFromAspNetUserId(int aspNetUserId);
    }
}
