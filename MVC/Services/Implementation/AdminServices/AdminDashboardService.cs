using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.Interfaces.AuthServices;
using Services.ViewModels.Admin;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Implementation.AdminServices
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AdminDashboardService(IRequestClientRepository requestClientRepository,IUserRepository userRepository,IJwtService jwtService)
        { 
            _requestClientRepository = requestClientRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public AdminDashboard getallRequests(int aspNetUserId)
        {
            List<Region> allRegion = _requestClientRepository.getAllRegions();
            Dictionary<int,string> regions = new Dictionary<int,string>();
            foreach (Region region in allRegion)
            {
                regions.Add(region.RegionId, region.Name);
            }
            List<CaseTag> caseTags = _requestClientRepository.getAllReason();
            Dictionary<int, string> reasons = new Dictionary<int, string>();
            foreach (CaseTag caseTag in caseTags)
            {
                reasons.Add(caseTag.CaseTagId, caseTag.Reason );
            }
            CancelPopUp cancelPopUp = new()
            {
                Reasons= reasons,
            };
            AssignAndTransferPopUp assignAndTransferPopUp = new()
            {
                Regions = regions,
            };
            AdminDashboard adminDashboard = new()
            {
                NewRequests = GetNewRequest(status:"New",pageNo:1),
                NewRequestCount = _requestClientRepository.countRequestClientByStatus(1),
                PendingRequestCount = _requestClientRepository.countRequestClientByStatus(2),
                ActiveRequestCount = _requestClientRepository.countRequestClientByStatus(4)  +
                                     _requestClientRepository.countRequestClientByStatus(5),
                ConcludeRequestCount = _requestClientRepository.countRequestClientByStatus(6),
                TocloseRequestCount = _requestClientRepository.countRequestClientByStatus(3) +
                                      _requestClientRepository.countRequestClientByStatus(7) +
                                      _requestClientRepository.countRequestClientByStatus(8),
                UnpaidRequestCount = _requestClientRepository.countRequestClientByStatus(9),
                Regions = regions,
                CancelPopup = cancelPopUp,
                AssignAndTransferPopup = assignAndTransferPopUp,
            };
            return adminDashboard;
        }

        public TableModel GetNewRequest(String status,int pageNo)
        {
            int skip = (pageNo - 1) * 10;
            int totalRequests = 0;
            List<RequestClient> requestClients= new List<RequestClient>();
            if (status=="New")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(1);
                requestClients = _requestClientRepository.getRequestClientByStatus(status:1,skip:skip);
            }
            else if(status == "Pending")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(2);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 2, skip: skip);
            }
            else if (status == "Active")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(4)+ _requestClientRepository.countRequestClientByStatus(5);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 4, skip: skip);
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(status: 5, skip: skip));
            }
            else if (status == "Conclude")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(6);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 6, skip: skip);
            }
            else if (status == "Close")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(3) + _requestClientRepository.countRequestClientByStatus(7)
                                               + _requestClientRepository.countRequestClientByStatus(8);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 3, skip: skip);
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(status: 7, skip: skip));
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(status: 8, skip: skip));
            }
            else if (status == "Unpaid")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(9);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 9, skip: skip);
            }
            return getTableModal(requestClients, totalRequests, pageNo);
            //
            //var allnewRequests = (from req in requests
            //                      join reqClient in requestClients
            //                      on req.RequestId equals reqClient.RequestId
            //                      where reqClient.Status == 1 
            //                      select new
            //                      {
            //                          req,
            //                          reqClient
            //                      }).ToList();
            //
        }

        public Dictionary<int, String> getPhysiciansByRegion(int regionId)
        {
            List<Physician> allPhysicians = _userRepository.getAllPhysiciansByRegionId(regionId);
            Dictionary<int, String> physicians = new Dictionary<int, String>();
            foreach (Physician physician in allPhysicians)
            {
                physicians.Add(physician.PhysicianId, physician.FirstName + " " + physician.LastName);
            }
            return physicians;
        }

        public Tuple<String, String, int> getRequestClientEmailAndMobile(int requestId)
        {
            RequestClient requestClient = _requestClientRepository.GetRequestClientAndRequestByRequestId(requestId);
            return new Tuple<string, string, int>(requestClient.Email, requestClient.PhoneNumber, requestClient.Request.RequestTypeId);
        }

        public Agreement getUserDetails(String token)
        {
            try
            {
                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(token);
                if (_jwtService.validateToken(token, out jwtSecurityToken))
                {
                    int requestId = int.Parse(jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == "requestId").Value);
                    RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
                    Agreement agreement = new Agreement()
                    {
                        FirstName = requestClient.FirstName,
                        LastName = requestClient.LastName,
                        RequestId = requestId,
                    };
                    return agreement;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public TableModel patientSearch(String patientName, String status, int pageNo, int type)
        {
            switch (type)
            {
                case 1: return searchOnPatientName(patientName, status, pageNo); break;
                case 2: return searchOnRegion(patientName, status, pageNo); break;
                default: return null;
            }
        }
        private TableModel searchOnPatientName(String patientName, String status, int pageNo)
        {
            patientName = patientName.ToLower();
            int totalRequests = 0;
            int skip = (pageNo - 1) * 10;
            List<RequestClient> requestClients = new List<RequestClient>();
            if (!patientName.Contains(" "))
            {
                patientName += " ";
            }
            String[] names = patientName.Split(" ");
            if (status == "New")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status:1);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 1);
            }
            else if (status == "Pending")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 2);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 2);
            }
            else if (status == "Active")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 4) + 
                                    _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 5);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 4);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 4, skip: skip);
                requestClients.AddRange(_requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 5));
            }
            else if (status == "Conclude")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 6);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 6);
            }
            else if (status == "Close")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 3) + 
                                   _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 7)
                                     + _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 8);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 3);
                requestClients.AddRange(_requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 7));
                requestClients.AddRange(_requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 8));
            }
            else if (status == "Unpaid")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 9);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 9);
            }
            return getTableModal(requestClients,totalRequests,pageNo);
        }

        private TableModel searchOnRegion(String patientName, String status, int pageNo)
        {
            patientName = patientName.ToLower();
            int totalRequests = 0;
            int skip = (pageNo - 1) * 10;
            List<RequestClient> requestClients = new List<RequestClient>();
            if (!patientName.Contains(" "))
            {
                patientName += " ";
            }
            String[] names = patientName.Split(" ");
            if (status == "New")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 1);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 1);
            }
            else if (status == "Pending")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 2);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 2);
            }
            else if (status == "Active")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 4) +
                                    _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 5);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 4);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 4, skip: skip);
                requestClients.AddRange(_requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 5));
            }
            else if (status == "Conclude")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 6);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 6);
            }
            else if (status == "Close")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 3) +
                                   _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 7)
                                     + _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 8);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 3);
                requestClients.AddRange(_requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 7));
                requestClients.AddRange(_requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 8));
            }
            else if (status == "Unpaid")
            {
                totalRequests = _requestClientRepository.countRequestClientByName(firstName: names[0], lastName: names[1], status: 9);
                requestClients = _requestClientRepository.getRequestClientByName(firstName: names[0], lastName: names[1], skip: skip, status: 9);
            }
            return getTableModal(requestClients, totalRequests, pageNo);
        }

        private TableModel getTableModal(List<RequestClient> requestClients,int totalRequests,int pageNo)
        {
            int skip = (pageNo - 1) * 10;
            List<TablesData> tablesDatas = new List<TablesData>();
            foreach (RequestClient requestClient in requestClients)
            {
                TablesData tablesData = new()
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
                    RegionId = requestClient.RegionId,
                    RequesterType = requestClient.Request.RequestTypeId,
                    BirthDate = requestClient.IntYear != null ? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth
                                 + "-" + requestClient.IntDate) : null,
                    RequestdDate = requestClient.Request.CreatedDate != null ? requestClient.Request.CreatedDate : null,
                    Email = requestClient.Email,
                    DateOfService = null,
                    PhysicianName = "",
                };
                tablesDatas.Add(tablesData);
            }
            int totalPages = totalRequests % 10 != 0 ? (totalRequests / 10) + 1 : totalRequests / 10;
            TableModel tableModel = new()
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
            return tableModel;
        }
    }
}
