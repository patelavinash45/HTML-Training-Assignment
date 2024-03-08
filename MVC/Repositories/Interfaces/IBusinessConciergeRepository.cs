using Repositories.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBusinessConciergeRepository
    {
        Task<int> addBusiness(Business business);

        Task<int> addConcierge(Concierge concierge);

        Task<int> addRequestConcierge(RequestConcierge requestConcierge);

        Task<int> addRequestBusiness(RequestBusiness requestBusiness);
    }
}
