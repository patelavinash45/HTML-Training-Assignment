using Repositories.ViewModels;

namespace Services.Interfaces.Patient
{
    public interface IPatientDashboardService
    {
        List<Dashboard> GetUsersMedicalData(int aspNetUserId);
    }
}
