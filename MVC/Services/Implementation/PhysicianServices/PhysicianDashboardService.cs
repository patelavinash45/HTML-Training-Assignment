using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.PhysicianServices;
using Services.ViewModels.Admin;
using Services.ViewModels.Physician;
using System.Collections;

namespace Services.Implementation.PhysicianServices
{
    public class PhysicianDashboardService : IPhysicianDashboardService
    {
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRequestStatusLogRepository _requestStatusLogRepository;
        private readonly IUserRepository _userRepository;

        private Dictionary<string, List<int>> statusList { get; set; } = new Dictionary<string, List<int>>()
            {
                {"new", new List<int> { 1 } },
                {"pending", new List<int> { 2 } },
                {"active", new List<int> { 4, 5 } },
                {"conclude", new List<int> { 6 } },
            };

        public PhysicianDashboardService(IRequestClientRepository requestClientRepository, IRequestStatusLogRepository requestStatusLogRepository,
                                                             IUserRepository userRepository)
        {
            _requestClientRepository = requestClientRepository;
            _requestStatusLogRepository = requestStatusLogRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> acceptRequest(int requestId)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(requestId);
            requestClient.Status = 2;
            return await _requestClientRepository.updateRequestClient(requestClient);
        }

        public async Task<bool> setEncounter(int requestId, bool isVideoCall)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientAndRequestByRequestId(requestId);
            requestClient.Status = isVideoCall ? 5 : 4;
            requestClient.Request.CallType = isVideoCall ? (short)1 : (short)2;
            return await _requestClientRepository.updateRequestClient(requestClient);
        }

        public int getPhysicianIdFromAspNetUserId(int aspNetUserId)
        {
            return _userRepository.getPhysicianByAspNetUserId(aspNetUserId).PhysicianId;
        }

        public async Task<bool> transferRequest(PhysicianTransferRequest model)
        {
            RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(model.RequestId);
            requestClient.Status = 1;
            requestClient.PhysicianId = null;
            if(await _requestClientRepository.updateRequestClient(requestClient))
            {
                return await _requestStatusLogRepository
                    .addRequestSatatusLog(
                    new RequestStatusLog()
                    {
                        RequestId = model.RequestId,
                        Status = 1,
                        CreatedDate = DateTime.Now,
                        Notes = model.TransferNotes,
                        TransToAdmin = new BitArray(1,true),
                    });
            }
            return false;
        }

        public PhysicianDashboard getallRequests(int aspNetUserId)
        {
            CancelPopUp cancelPopUp = new()
            {
                Reasons = _requestClientRepository.getAllReason().ToDictionary(caseTag => caseTag.CaseTagId, caseTag => caseTag.Reason),
            };
            AssignAndTransferPopUp assignAndTransferPopUp = new()
            {
                Regions = _requestClientRepository.getAllRegions().ToDictionary(region => region.RegionId, region => region.Name),
            };
            return new PhysicianDashboard()
            {
                NewRequests = GetNewRequest(status: "new", pageNo: 1, patientName: "", regionId: 0, requesterTypeId: 0, aspNetUserId),
                NewRequestCount = _requestClientRepository.countRequestClientByStatusForPhysician(statusList["new"],aspNetUserId),
                PendingRequestCount = _requestClientRepository.countRequestClientByStatusForPhysician(statusList["pending"], aspNetUserId),
                ActiveRequestCount = _requestClientRepository.countRequestClientByStatusForPhysician(statusList["active"], aspNetUserId),
                ConcludeRequestCount = _requestClientRepository.countRequestClientByStatusForPhysician(statusList["conclude"], aspNetUserId),
                Regions = _requestClientRepository.getAllRegions().ToDictionary(region => region.RegionId, region => region.Name),
            };
        }

        public TableModel GetNewRequest(String status, int pageNo, String patientName, int regionId, int requesterTypeId, int aspNetUserId)
        {
            int skip = (pageNo - 1) * 10;
            Func<RequestClient, bool> predicate = a =>
            (requesterTypeId == 0 || a.Request.RequestTypeId == requesterTypeId)
            && (regionId == 0 || a.RegionId == regionId)
            && (patientName == null || a.FirstName.ToLower().Contains(patientName) || a.LastName.ToLower().Contains(patientName))
            && (!statusList[status].Contains(1) || a.Physician != null)
            && a.Physician != null
            && a.Physician.AspNetUserId == aspNetUserId
            && (statusList[status].Contains(a.Status));
            int totalRequests = _requestClientRepository.countRequestClientByStatusAndFilter(predicate);
            List<RequestClient> requestClients = _requestClientRepository.getRequestClientByStatus(predicate, skip: skip);
            return getTableModal(requestClients, totalRequests, pageNo);
        }

        private TableModel getTableModal(List<RequestClient> requestClients, int totalRequests, int pageNo)
        {
            int skip = (pageNo - 1) * 10;
            List<TablesData> tablesDatas = requestClients
                .Select(requestClient => new TablesData()
                {
                    RequestId = requestClient.RequestId,
                    FirstName = requestClient.FirstName,
                    LastName = requestClient.LastName,
                    Requester = requestClient.Request.RequestTypeId,
                    RequesterFirstName = requestClient.Request.FirstName,
                    RequesterLastName = requestClient.Request.LastName,
                    Mobile = requestClient.PhoneNumber,
                    RequesterMobile = requestClient.Request.PhoneNumber,
                    State = requestClient.State,
                    Street = requestClient.Street,
                    ZipCode = requestClient.ZipCode,
                    City = requestClient.City,
                    Notes = requestClient.Symptoms,
                    RequesterType = requestClient.Request.RequestTypeId,
                    Email = requestClient.Email,
                    IsEncounter = requestClient.Request.CallType != null ? 1 : 0,
                    EncounterType = requestClient.Request.CallType,
                }).ToList();
            int totalPages = totalRequests % 10 != 0 ? (totalRequests / 10) + 1 : totalRequests / 10;
            return new TableModel()
            {
                IsFirstPage = pageNo != 1,
                IsLastPage = pageNo != totalPages,
                IsNextPage = pageNo < totalPages,
                IsPreviousPage = pageNo > 1,
                TableDatas = tablesDatas,
                TotalRequests = totalRequests,
                PageNo = pageNo,
                StartRange = skip + 1,
                EndRange = skip + 10 < totalRequests ? skip + 10 : totalRequests,
            };
        }
    }
}
