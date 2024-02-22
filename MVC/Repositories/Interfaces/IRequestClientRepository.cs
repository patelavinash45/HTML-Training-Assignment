using Repositories.DataModels;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRequestClientRepository
    {
        List<RequestClient> getRequestClientByStatus(int status);

        List<RequestClient> getAllRequestClientForUser(int userId);

        Task<int> addRequestClient(RequestClient requestClient);

        RequestClient GetRequestClientByRequestId(int requestId);
    }
}
