using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels;
using Services.ViewModels.Admin;

namespace Services.Implementation.AdminServices
{
    public class CloseCaseService : ICloseCaseService
    {
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRequestWiseFileRepository _requestWiseFileRepository;

        public CloseCaseService(IRequestClientRepository requestClientRepository, IRequestWiseFileRepository requestWiseFileRepository)
        {
            _requestClientRepository = requestClientRepository;
            _requestWiseFileRepository = requestWiseFileRepository;
        }

        public CloseCase getDaetails(int requestId)
        {
            List<RequestWiseFile> requestWiseFiles = _requestWiseFileRepository.getFilesByrequestId(requestId);
            List<FileModel> fileList = new List<FileModel>();
            foreach (var file in requestWiseFiles)
            {
                FileModel fileModel = new FileModel()
                {
                    RequestId = requestId,
                    RequestWiseFileId = file.RequestWiseFileId,
                    FileName = file.FileName,
                    Uploder = file.Uploder,
                    CreatedDate = file.CreatedDate,
                };
                fileList.Add(fileModel);
            };
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(requestId);
            CloseCase closeCase = new CloseCase()
            {
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                Phone = requestClient.PhoneNumber,
                Email = requestClient.Email,
                BirthDate = requestClient.IntDate != null ? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth
                                 + "-" + requestClient.IntDate) : null,
                FileList = fileList,
            };
            return closeCase;
        }

        public async Task<bool> updateDetails(CloseCase model,int requestId)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(requestId);
            requestClient.FirstName = model.FirstName;
            requestClient.LastName = model.LastName;
            requestClient.PhoneNumber = model.Phone;
            requestClient.IntDate = model.BirthDate.Value.Day;
            requestClient.IntYear = model.BirthDate.Value.Year;
            requestClient.StrMonth = model.BirthDate.Value.Month.ToString();
            return await _requestClientRepository.updateRequestClient(requestClient);
        }

        public async Task<bool> requestAddToCloseCase(int requestId)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(requestId);
            requestClient.Status = 8;
            return await _requestClientRepository.updateRequestClient(requestClient);
        }
    }
}
