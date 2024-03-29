using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels.Admin;

namespace Services.Implementation.AdminServices
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
            Dictionary<int, string> reasons = _requestClientRepository.getAllReason().ToDictionary(caseTag => caseTag.CaseTagId,caseTag => caseTag.Reason);
            CancelPopUp cancelPopUp = new()
            {
                Reasons = reasons,
            };
            RequestClient requestClient = _requestClientRepository.getRequestClientAndRequestByRequestId(requestId);
            ViewCase viewCase = new() 
            { 
                Status = requestClient.Status,
                Requester = requestClient.Request.RequestTypeId,
                RequestId = requestId,
                PatientNotes = requestClient.Symptoms,
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                BirthDate= requestClient.IntYear != null ? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth
                                 + "-" + requestClient.IntDate) : null,
                Mobile=requestClient.PhoneNumber,
                Email=requestClient.Email,
                Address=requestClient.Street+ " " +requestClient.City + " " + requestClient.State + " " + requestClient.ZipCode,
                Region=requestClient.State,
                CancelPopup=cancelPopUp,
            };
            return viewCase;
        }

        public async Task<bool> updateRequest(ViewCase model)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(model.RequestId);
            requestClient.Symptoms = model.PatientNotes;
            requestClient.FirstName = model.FirstName;
            requestClient.LastName = model.LastName;
            requestClient.PhoneNumber = model.Mobile;
            requestClient.IntYear = model.BirthDate.Value.Year;
            requestClient.IntDate= model.BirthDate.Value.Day;
            requestClient.StrMonth = model.BirthDate.Value.Month.ToString();
            return await _requestClientRepository.updateRequestClient(requestClient);
        }

    }       
}
