using Repositories.DataModels;
using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface ISendOrderService
    {
        SendOrder getSendOrderDetails(int requestId);

        HealthProfessional getBussinessData(int venderId);

        Task<int> addOrderDetails(SendOrder model); 
    }
}
