using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Patient
{
    public interface IViewDocumentsServices
    {
        ViewDocument getDocumentList(int requestId,int aspNetUserId);

        Task<bool> uploadFile(ViewDocument model);
    }
}
