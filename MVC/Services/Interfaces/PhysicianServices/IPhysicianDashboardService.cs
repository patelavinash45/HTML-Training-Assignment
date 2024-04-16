using Services.ViewModels.Admin;
using Services.ViewModels.Physician;

namespace Services.Interfaces.PhysicianServices
{
    public interface IPhysicianDashboardService
    {
        PhysicianDashboard getallRequests(int aspNetUserId);

        TableModel GetNewRequest(String status, int pageNo, String patientName, int regionId, int requesterTypeId, int aspNetUserId);
    }
}
