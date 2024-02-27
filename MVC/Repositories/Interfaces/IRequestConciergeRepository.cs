using Repositories.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRequestConciergeRepository
    {
        Task<int> addRequestConcierge(RequestConcierge requestConcierge);
    }
}
