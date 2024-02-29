﻿using Repositories.DataModels;
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
            _requestClientRepository= requestClientRepository;
        }
        public async Task<ViewNotes> GetNotes(int RequestId)
        {
            RequestNote requestNote = _requestNotesRepository.GetRequestNoteByRequestId(RequestId);
            List<RequestStatusLog> requestStatusLogs = _requestSatatusLogRepository.GetRequestStatusLogByRequestId(RequestId);
            List<string> transferNotes = new List<string>();
            foreach(var requestStatusLog in requestStatusLogs)
            {
                transferNotes.Add(requestStatusLog.Notes);
            };
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
                TransferNotes = transferNotes,
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

        public async Task<bool> cancleRequest(CancelPopUp model)
        {
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(model.RequestId);
            requestClient.Status = 3;
            await _requestClientRepository.updateRequestClient(requestClient);
            Request request = _requestRepository.getRequestByRequestId(model.RequestId);
            request.CaseTagId = model.Reason;
            await _requestRepository.updateRequest(request);
            RequestStatusLog _requestStatusLog = new()
            {
                RequestId = model.RequestId,
                Status = 3,
                CreatedDate = DateTime.Now,
                Notes = model.AdminTransferNotes,
            };
            return await _requestSatatusLogRepository.addRequestSatatusLog(_requestStatusLog) > 0;
        }

        public async Task<bool> assignRequest(AssignPopUp model)
        {
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(model.RequestId);
            requestClient.Status = 2;
            requestClient.PhysicianId = model.SelectedPhysician;
            await _requestClientRepository.updateRequestClient(requestClient);
            RequestStatusLog _requestStatusLog = new()
            {
                RequestId = model.RequestId,
                Status = 2,
                CreatedDate = DateTime.Now,
                Notes = model.AdminTransferNotes,
                PhysicianId = model.SelectedPhysician,
            };
            return await _requestSatatusLogRepository.addRequestSatatusLog(_requestStatusLog) > 0;
        }
    }
}