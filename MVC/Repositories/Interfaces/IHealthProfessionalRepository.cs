using Repositories.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IHealthProfessionalRepository
    {
        List<HealthProfessionalType> getHealthProfessionalTypes();

        List<HealthProfessional> getHealthProfessional();

        HealthProfessional getHealthProfessional(int VenderId);

        Task<int> addOrderDetails(OrderDetail orderDetail);
    }
}
