﻿using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.Interfaces.AuthServices;
using Services.ViewModels.Admin;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace Services.Implementation.AdminServices
{
    public class ViewNotesService : IViewNotesService
    {
        private readonly IRequestNotesRepository _requestNotesRepository;
        private readonly IRequestStatusLogRepository _requestSatatusLogRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IJwtService _jwtService;

        public ViewNotesService(IRequestNotesRepository requestNotesRepository, IRequestStatusLogRepository requestSatatusLogRepository, 
                                      IRequestClientRepository requestClientRepository, IRequestRepository requestRepository, 
                                      IJwtService jwtService)
        {
            _requestNotesRepository = requestNotesRepository;
            _requestSatatusLogRepository = requestSatatusLogRepository;
            _requestRepository = requestRepository;
            _requestClientRepository = requestClientRepository;
            _jwtService = jwtService;
        }
        public ViewNotes GetNotes(int RequestId)
        {
            RequestNote requestNote = _requestNotesRepository.GetRequestNoteByRequestId(RequestId);
            List<RequestStatusLog> requestStatusLogs = _requestSatatusLogRepository.GetRequestStatusLogByRequestId(RequestId);
            List<string> transferNotes = new List<string>();
            foreach(var requestStatusLog in requestStatusLogs)
            {
                transferNotes.Add(requestStatusLog.Notes);
            };
            ViewNotes notes = new()
            {
                RequestId = RequestId,
                AdminNotes = requestNote!=null?requestNote.AdminNotes:null,
                PhysicianNotes = requestNote != null ? requestNote.PhysicianNotes : null,
                TransferNotes = transferNotes,
            };
            return notes;
        }

        public async Task<bool> addAdminNotes(String adminNotes, int requestId)
        {
            RequestNote requestNote = _requestNotesRepository.GetRequestNoteByRequestId(requestId);
            if(requestNote == null)
            {
                RequestNote _requestNote = new()
                {
                    RequestId = requestId,
                    AdminNotes = adminNotes,
                    CreatedDate = DateTime.Now,
                };
                return await _requestNotesRepository.addRequestNote(_requestNote);
            }
            requestNote.AdminNotes= adminNotes;
            return await _requestNotesRepository.updateRequestNote(requestNote);
        }

        public async Task<bool> cancleRequest(CancelPopUp model)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(model.RequestId);
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
            return await _requestSatatusLogRepository.addRequestSatatusLog(_requestStatusLog);
        }

        public async Task<bool> agreementDeclined(Agreement model)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(model.RequestId);
            requestClient.Status = 8;
            await _requestClientRepository.updateRequestClient(requestClient);
            RequestStatusLog _requestStatusLog = new()
            {
                RequestId = model.RequestId,
                Status = 8,
                CreatedDate = DateTime.Now,
                Notes = model.CancelationReson,
            };
            return await _requestSatatusLogRepository.addRequestSatatusLog(_requestStatusLog);
        }

        public async Task<bool> agreementAgree(Agreement model)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(model.RequestId);
            requestClient.Status = 4;
            return await _requestClientRepository.updateRequestClient(requestClient);
        }

        public async Task<bool> assignRequest(AssignAndTransferPopUp model)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(model.RequestId);
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
            return await _requestSatatusLogRepository.addRequestSatatusLog(_requestStatusLog);
        }

        public async Task<bool> blockRequest(BlockPopUp model)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(model.RequestId);
            requestClient.Status = 0;
            await _requestClientRepository.updateRequestClient(requestClient);
            BlockRequest blockRequest = new()
            {
                PhoneNumber = requestClient.PhoneNumber,
                Email = requestClient.Email,
                Reason = model.AdminTransferNotes,
                RequestId = model.RequestId,
                CreatedDate = DateTime.Now,
            };
            await _requestSatatusLogRepository.addBlockRequest(blockRequest);
            RequestStatusLog _requestStatusLog = new()
            {
                RequestId = model.RequestId,
                Status = 0,
                CreatedDate = DateTime.Now,
                Notes = model.AdminTransferNotes,
            };
            return await _requestSatatusLogRepository.addRequestSatatusLog(_requestStatusLog);
        }

        public async Task<bool> clearRequest(int requestId)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(requestId);
            requestClient.Status = 12;
            return await _requestClientRepository.updateRequestClient(requestClient);
        }

        public bool sendAgreement(Agreement model)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("requestId", model.RequestId.ToString()),
            };
            String token = _jwtService.genrateJwtTokenForSendMail(claims, DateTime.Now.AddDays(2));
            String link = "https://localhost:44392/Admin/Agreement?token=" + token;
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                Subject = "Agreement",
                IsBodyHtml = true,
                Body = "To Further Proceed to your Request : " + link,
            };
            //mailMessage.To.Add(model.Email);
            mailMessage.To.Add("tatva.dotnet.avinashpatel@outlook.com");
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com")
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Port = 587,
                Credentials = new NetworkCredential(userName: "tatva.dotnet.avinashpatel@outlook.com", password: "Avinash@6351"),
            };
            try
            {
                smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
