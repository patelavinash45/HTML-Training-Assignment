using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels.Admin;
using Services.Interfaces.Admin;

namespace Services.Implementation.Admin
{
    public class ViewCaseService : IViewCaseService
    {
        private readonly IRequestClientRepository _requestClientRepository;

        public ViewCaseService(IRequestClientRepository requestClientRepository)
        {
            _requestClientRepository = requestClientRepository;
        }

        public ViewCase getRequestDetails(int requestId)
        {
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
            ViewCase viewCase = new() 
            { 
                RequestId = requestId,
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                BirthDate= requestClient.IntYear != null ? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth
                                 + "-" + requestClient.IntDate) : null,
                Mobile=requestClient.PhoneNumber,
                Email=requestClient.Email,
                Address=requestClient.Street+ " " +requestClient.City + " " + requestClient.State + " " + requestClient.ZipCode,
                Region=requestClient.State,
            };
            return viewCase;
        }

        public async Task<bool> updateRequest(ViewCase model)
        {
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(model.RequestId);
            requestClient.FirstName = model.FirstName;
            requestClient.LastName = model.LastName;
            requestClient.PhoneNumber = model.Mobile;
            requestClient.IntYear = model.BirthDate.Value.Year;
            requestClient.IntDate= model.BirthDate.Value.Day;
            requestClient.StrMonth = model.BirthDate.Value.Month.ToString();
            return await _requestClientRepository.updateRequestClient(requestClient);
        }

        public async Task<bool> cancelRequest(int requestId)
        {
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
            requestClient.Status = 3;
            return await _requestClientRepository.updateRequestClient(requestClient);
        }
    }       
}
