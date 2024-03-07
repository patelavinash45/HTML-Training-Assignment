using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels;
using Services.ViewModels.Admin;

namespace Services.Implementation.AdminServices
{
    public class ViewCaseService : IViewCaseService
    {
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly ICaseTagRepository _caseTagRepository;

        public ViewCaseService(IRequestClientRepository requestClientRepository, ICaseTagRepository caseTagRepository)
        {
            _requestClientRepository = requestClientRepository;
            _caseTagRepository = caseTagRepository;
        }

        public ViewCase getRequestDetails(int requestId)
        {
            List<CaseTag> caseTags = _caseTagRepository.getAllReason();
            Dictionary<int, string> reasons = new Dictionary<int, string>();
            foreach (CaseTag caseTag in caseTags)
            {
                reasons.Add(caseTag.CaseTagId, caseTag.Reason);
            }
            CancelPopUp cancelPopUp = new()
            {
                Reasons = reasons,
            };
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
                CancelPopup=cancelPopUp,
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

    }       
}
