using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Repositories.ViewModels.Admin;
using Services.Interfaces.Admin;

namespace Services.Implementation.Admin
{
    public class ViewNotesService : IViewNotesService
    {
        private readonly IRequestNotesRepository _requestNotesRepository;
        private readonly IRequestSatatusLogRepository _requestSatatusLogRepository;

        public ViewNotesService(IRequestNotesRepository requestNotesRepository, IRequestSatatusLogRepository requestSatatusLogRepository)
        {
            _requestNotesRepository = requestNotesRepository;
            _requestSatatusLogRepository = requestSatatusLogRepository;
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
    }
}
