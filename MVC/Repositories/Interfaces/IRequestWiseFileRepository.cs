using Repositories.DataModels;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRequestWiseFileRepository
    {
        int countFile(int requestId);

        Task<int> addFile(RequestWiseFile requestWiseFile);

        List<RequestWiseFile> getFilesByrequestId(int requestId);
    }
}
