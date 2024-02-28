using Repositories.DataModels;
using Repositories.Implementation;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Repositories.ViewModels.Admin;
using Services.Interfaces.Admin;

namespace Services.Implementation.Admin
{
    public class ViewNotesService : IViewNotesService
    {
        private readonly IRequestNotesRepository _requestNotesRepository;
        private readonly IRequestStatusLogRepository _requestSatatusLogRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRequestRepository _requestRepository;


        public ViewNotesService(IRequestNotesRepository requestNotesRepository, IRequestStatusLogRepository requestSatatusLogRepository, 
                                      IRequestClientRepository requestClientRepository, IRequestRepository requestRepository)
        {
            _requestNotesRepository = requestNotesRepository;
            _requestSatatusLogRepository = requestSatatusLogRepository;
            _requestRepository = requestRepository;
            _requestRepository = requestRepository;
        }
        public async Task<ViewNotes> GetNotes(int RequestId)
        {
            RequestNote requestNote = _requestNotesRepository.GetRequestNoteByRequestId(RequestId);
            RequestStatusLog requestStatusLog = _requestSatatusLogRepository.GetRequestStatusLogByRequestId(RequestId);
            DashboardHeader dashboardHeader = new()
            {
                PageType = 1,
            };
            ViewNotes notes = new()
            {
                Header = dashboardHeader,
                RequestId = RequestId,
                AdminNotes = requestNote!=null?requestNote.AdminNotes:null,
                PhysicianNotes = requestNote != null ? requestNote.PhysicianNotes : null,
                TransferNotes = requestStatusLog!=null ? requestStatusLog.Notes: null,
            };
            return notes;
        }

        public async Task<bool> addAdminNotes(ViewNotes model)
        {
            RequestNote requestNote = _requestNotesRepository.GetRequestNoteByRequestId(model.RequestId);
            if(requestNote == null)
            {
                RequestNote _requestNote = new()
                {
                    RequestId = model.RequestId,
                    AdminNotes = model.NewAdminNotes,
                    CreatedDate = DateTime.Now,
                };
                return await _requestNotesRepository.addRequestNote(_requestNote) > 0;
            }
            requestNote.AdminNotes= model.NewAdminNotes;
            return await _requestNotesRepository.updateRequestNote(requestNote);
        }

        public async Task<bool> addAdminTransform(CancelPopUp model)
        {
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(model.RequestId);
            requestClient.Status = 3;
            await _requestClientRepository.updateRequestClient(requestClient);
            Request request = _requestRepository.getRequestByRequestId(model.RequestId);
            request.CaseTagId = model.Reason;
            await _requestRepository.updateRequest(request);
            RequestStatusLog requestStatusLog = _requestSatatusLogRepository.GetRequestStatusLogByRequestId(model.RequestId);
            if (requestStatusLog == null)
            {
                RequestStatusLog _requestStatusLog = new()
                {
                    RequestId = model.RequestId,
                    Status = 3,
                    CreatedDate = DateTime.Now,
                    Notes = model.AdminTransferNotes,
                };
                return await _requestSatatusLogRepository.addRequestSatatusLog(_requestStatusLog) > 0;
            }
            requestStatusLog.Notes = model.AdminTransferNotes;
            return await _requestSatatusLogRepository.updateRequestSatatusLog(requestStatusLog);
        }
    }
}
