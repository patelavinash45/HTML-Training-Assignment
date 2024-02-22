using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation.Patient
{
    public class ViewDocumentsServices :IViewDocumentsServices
    {
        private readonly IRequestWiseFileRepository _requestWiseFileRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IFileService _fileService;

        public ViewDocumentsServices(IRequestWiseFileRepository requestWiseFileRepository, IRequestClientRepository requestClientRepository, 
                                      IFileService fileService)
        {
            _requestWiseFileRepository = requestWiseFileRepository;
            _requestClientRepository = requestClientRepository;
            _fileService = fileService;
        }

        public ViewDocument getDocumentList(int requestId,int aspNetUserId)
        {
            List<RequestWiseFile> requestWiseFiles= _requestWiseFileRepository.getFilesByrequestId(requestId);
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
            DashboardHeader dashboardHeader = new()
            {
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                AspNetUserId = aspNetUserId,
            };
            ViewDocument viewDocument = new()
            {
                FileList = requestWiseFiles,
                Header = dashboardHeader,
                RequestId = requestClient.RequestId,
            };
            return viewDocument;
        }

        public Task<bool> uploadFile(ViewDocument model)
        {
            await _fileService.addFile(requestId: model.RequestId, firstName: model.Header.FirstName, lastName: model.Header.LastName);
        }
    }
}
