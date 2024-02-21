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
        List<RequestClient> getAllRequestClient();

        Task<int> addRequestClient(int userId, AddPatientRequest model, int requestId, int regionId);
    }
}
