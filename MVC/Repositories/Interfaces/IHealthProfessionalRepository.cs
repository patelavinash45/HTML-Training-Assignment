using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IHealthProfessionalRepository
    {
        List<HealthProfessionalType> getHealthProfessionalTypes();

        List<HealthProfessional> getHealthProfessionalByProfession(int professionId);

        HealthProfessional getHealthProfessional(int VenderId);

        Task<bool> addOrderDetails(OrderDetail orderDetail);
    }
}
