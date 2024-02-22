using Repositories.DataModels;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRequestRepository
    {
        List<Request> getAllRequest();

        Task<int> addRequest(Request request);
    }
}
