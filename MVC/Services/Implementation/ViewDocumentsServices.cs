
using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces;
using System.Collections;

namespace Services.Implementation
{
    public class ViewDocumentsServices : IViewDocumentsServices
    {
        private readonly IRequestWiseFileRepository _requestWiseFileRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IFileService _fileService;
        private readonly IAdminRepository _adminRepository;

        public ViewDocumentsServices(IRequestWiseFileRepository requestWiseFileRepository, IRequestClientRepository requestClientRepository,
                                      IFileService fileService, IAdminRepository adminRepository)
        {
            _requestWiseFileRepository = requestWiseFileRepository;
            _requestClientRepository = requestClientRepository;
            _fileService = fileService;
            _adminRepository = adminRepository;
        }

        public ViewDocument getDocumentList(int requestId, int aspNetUserId)
        {
            List<RequestWiseFile> requestWiseFiles = _requestWiseFileRepository.getFilesByrequestId(requestId);
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
            Admin admin = _adminRepository.getAdmionByAspNetUserId(aspNetUserId);
            DashboardHeader dashboardHeader = new()
            {
                FirstName = admin.FirstName, 
                LastName = admin.LastName,
                AspNetUserId = aspNetUserId,
            };
            ViewDocument viewDocument = new()
            {
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                FileList = requestWiseFiles,
                Header = dashboardHeader,
                RequestId = requestClient.RequestId,
            };
            return viewDocument;
        }

        public async Task<int> uploadFile(ViewDocument model)
        {
            return await _fileService.addFile(requestId: model.RequestId, firstName: model.Header.FirstName, lastName: model.Header.LastName,
                                                      file: model.File);
        }

        public async Task<int> deleteFile(int requestWiseFileId)
        {
            RequestWiseFile requestWiseFile = _requestWiseFileRepository.getFilesByrequestWiseFileId(requestWiseFileId);
            requestWiseFile.IsDeleted = new BitArray(1, true);
            if(_fileService.removeFile(requestWiseFile.FileName))
            {
                return await _requestWiseFileRepository.updateRequestWiseFile(requestWiseFile) ? requestWiseFile.RequestId : 0;
            }
            return 0;
        }

        public async Task<int> deleteAllFile(List<RequestWiseFile> requestWiseFileIds)
        {
            int requestId = 0;
            foreach(var item in requestWiseFileIds)
            {
                requestId = await deleteFile(item.RequestWiseFileId);
            }
            return requestId;
        }
    }
}
