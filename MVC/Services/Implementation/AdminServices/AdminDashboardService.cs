using Microsoft.AspNetCore.Http;
using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.Interfaces.AuthServices;
using Services.ViewModels.Admin;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation.AdminServices
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IAspRepository _aspRepository;

        public AdminDashboardService(IRequestClientRepository requestClientRepository, IUserRepository userRepository, IJwtService jwtService,
                                          IAspRepository aspRepository, IRequestRepository requestRepository)
        {
            _requestClientRepository = requestClientRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _aspRepository = aspRepository;
            _requestRepository = requestRepository;
        }

        public AdminDashboard getallRequests(int aspNetUserId)
        {
            List<Region> allRegion = _requestClientRepository.getAllRegions();
            Dictionary<int, string> regions = new Dictionary<int, string>();
            foreach (Region region in allRegion)
            {
                regions.Add(region.RegionId, region.Name);
            }
            List<CaseTag> caseTags = _requestClientRepository.getAllReason();
            Dictionary<int, string> reasons = new Dictionary<int, string>();
            foreach (CaseTag caseTag in caseTags)
            {
                reasons.Add(caseTag.CaseTagId, caseTag.Reason);
            }
            CancelPopUp cancelPopUp = new()
            {
                Reasons = reasons,
            };
            AssignAndTransferPopUp assignAndTransferPopUp = new()
            {
                Regions = regions,
            };
            AdminDashboard adminDashboard = new()
            {
                NewRequests = GetNewRequest(status: "New", pageNo: 1),
                NewRequestCount = _requestClientRepository.countRequestClientByStatus(1),
                PendingRequestCount = _requestClientRepository.countRequestClientByStatus(2),
                ActiveRequestCount = _requestClientRepository.countRequestClientByStatus(4) +
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

        public TableModel GetNewRequest(String status, int pageNo)
        {
            int skip = (pageNo - 1) * 10;
            int totalRequests = 0;
            List<RequestClient> requestClients = new List<RequestClient>();
            if (status == "New")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(1);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 1, skip: skip);
            }
            else if (status == "Pending")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(2);
                requestClients = _requestClientRepository.getRequestClientByStatus(status: 2, skip: skip);
            }
            else if (status == "Active")
            {
                totalRequests = _requestClientRepository.countRequestClientByStatus(4) + _requestClientRepository.countRequestClientByStatus(5);
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
            RequestClient requestClient = _requestClientRepository.getRequestClientAndRequestByRequestId(requestId);
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
                    RequestClient requestClient = _requestClientRepository.getRequestClientByRequestId(requestId);
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

        public bool SendRequestLink(SendLink model,HttpContext httpContext)
        {
            var request = httpContext.Request;
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                Subject = "Link For Patient Request",
                IsBodyHtml = true,
                Body = "Link For Patient Request: "+request.Scheme+"://"+request.Host+"/Patient/PatientRequest",
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

        public async Task<bool> createRequest(CreateRequest model, int aspNetUserIdAdmin)
        {
            int aspNetUserId = _aspRepository.checkUser(email: model.Email);
            int userId = _userRepository.getUserID(aspNetUserId);
            if (aspNetUserId == 0)
            {
                AspNetUser aspNetUser = new()
                {
                    UserName = model.FirstName,
                    Email = model.Email,
                    PhoneNumber = model.Mobile,
                    PasswordHash = genrateHash(model.Password),
                    CreatedDate = DateTime.Now,
                };
                aspNetUserId = await _aspRepository.addUser(aspNetUser);
                User user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    AspNetUserId = aspNetUserId,
                    CreatedBy = aspNetUserId,
                    CreatedDate = DateTime.Now,
                    IntYear = model.BirthDate.Value.Year,
                    IntDate = model.BirthDate.Value.Day,
                    StrMonth = model.BirthDate.Value.Month.ToString(),
                };
                userId = await _userRepository.addUser(user);
                AspNetUserRole aspNetUserRole = new()
                {
                    UserId = aspNetUserId,
                    RoleId = _aspRepository.checkUserRole(role: "Patient"),
                };
                await _aspRepository.addAspNetUserRole(aspNetUserRole);
            }
            Admin admin = _userRepository.getAdmionByAspNetUserId(aspNetUserIdAdmin);
            Request request = new()
            {
                RequestTypeId = 5,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                PhoneNumber = admin.Mobile,
                UserId = userId,
                CreatedDate = DateTime.Now,
            };
            int requestId = await _requestRepository.addRequest(request);
            RequestClient requestClient = new()
            {
                RequestId = requestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
                Email = model.Email,
                State = model.State,
                Street = model.Street,
                City = model.City,
                ZipCode = model.ZipCode,
                Status = 1,
                Symptoms = model.Symptoms,
                IntYear = DateTime.Now.Year,
                IntDate = DateTime.Now.Day,
                StrMonth = DateTime.Now.Month.ToString(),
            };
            return await _requestClientRepository.addRequestClient(requestClient) == 0 ? false : true;
        }

        public bool RequestSupport(RequestSupport model)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                Subject = "Request DTY Support",
                IsBodyHtml = true,
                Body = "We are short on coverage and needs additional support On Call to respond to Requests. And Admin Message :: " + model.Message
            };
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com")
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Port = 587,
                Credentials = new NetworkCredential(userName: "tatva.dotnet.avinashpatel@outlook.com", password: "Avinash@6351"),
            };
            //List<Physician> physicians = _userRepository.getAllUnAssignedPhysician();
            //foreach (Physician physician in physicians)
            //{
            //    mailMessage.Bcc.Add(physician.Email);
            //}
            mailMessage.To.Add("tatva.dotnet.avinashpatel@outlook.com");
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

        public DataTable exportData(int pageNo, String status, int type, String searchElement)
        {
            switch(type)
            {
                case 1: return convertRequestClientToDataTable(GetNewRequest(status, pageNo).TableDatas);
                case 2: case 3: case 4: return convertRequestClientToDataTable(patientSearch(searchElement ,status, pageNo, type).TableDatas);
                default: return null;
            }
        }

        public DataTable exportAllData()
        {
            List<RequestClient> requestClients = _requestClientRepository.getAllRequestClients();
            return convertRequestClientToDataTable(requestClients);
        }

        public TableModel patientSearch(String searchElement, String status, int pageNo, int type)
        {
            switch (type)
            {
                case 2: return searchOnPatientName(searchElement, status, pageNo);
                case 3: return searchOnRegion(searchElement, status, pageNo);
                case 4: return searchOnRequesttype(searchElement, status, pageNo); 
                default: return null;
            }
        }
        private TableModel searchOnPatientName(String searchElement, String status, int pageNo)
        {
            int totalRequests = 0;
            int skip = (pageNo - 1) * 10;
            List<RequestClient> requestClients = new List<RequestClient>();
            searchElement = searchElement.ToLower();
            if (!searchElement.Contains(" "))
            {
                searchElement += " ";
            }
            String[] names = searchElement.Split(" ");
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

        private TableModel searchOnRegion(String searchElement, String status, int pageNo)
        {
            int regionId = int.Parse(searchElement);
            int totalRequests = 0;
            int skip = (pageNo - 1) * 10;
            List<RequestClient> requestClients = new List<RequestClient>();
            if (status == "New")
            {
                totalRequests = _requestClientRepository.countRequestClientByRegion(regionId, status: 1);
                requestClients = _requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 1);
            }
            else if (status == "Pending")
            {
                totalRequests = _requestClientRepository.countRequestClientByRegion(regionId, status: 2);
                requestClients = _requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 2);
            }
            else if (status == "Active")
            {
                totalRequests = _requestClientRepository.countRequestClientByRegion(regionId, status: 4) +
                                    _requestClientRepository.countRequestClientByRegion(regionId, status: 5);
                requestClients = _requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 4);
                requestClients.AddRange(_requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 5));
            }
            else if (status == "Conclude")
            {
                totalRequests = _requestClientRepository.countRequestClientByRegion(regionId, status: 6);
                requestClients = _requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 6);
            }
            else if (status == "Close")
            {
                totalRequests = _requestClientRepository.countRequestClientByRegion(regionId, status: 3) +
                                   _requestClientRepository.countRequestClientByRegion(regionId, status: 7)
                                     + _requestClientRepository.countRequestClientByRegion(regionId, status: 8);
                requestClients = _requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 3);
                requestClients.AddRange(_requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 7));
                requestClients.AddRange(_requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 8));
            }
            else if (status == "Unpaid")
            {
                totalRequests = _requestClientRepository.countRequestClientByRegion(regionId, status: 9);
                requestClients = _requestClientRepository.getRequestClientByRegion(regionId, skip: skip, status: 9);
            }
            return getTableModal(requestClients, totalRequests, pageNo);
        }

        private TableModel searchOnRequesttype(String searchElement, String status, int pageNo)
        {
            int requestTypeId = int.Parse(searchElement);
            int totalRequests = 0;
            int skip = (pageNo - 1) * 10;
            List<RequestClient> requestClients = new List<RequestClient>();
            if (status == "New")
            {
                totalRequests = _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 1);
                requestClients = _requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 1);
            }
            else if (status == "Pending")
            {
                totalRequests = _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 2);
                requestClients = _requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 2);
            }
            else if (status == "Active")
            {
                totalRequests = _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 4) +
                                    _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 5);
                requestClients = _requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 4);
                requestClients.AddRange(_requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 5));
            }
            else if (status == "Conclude")
            {
                totalRequests = _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 6);
                requestClients = _requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 6);
            }
            else if (status == "Close")
            {
                totalRequests = _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 3) +
                                   _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 7)
                                     + _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 8);
                requestClients = _requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 3);
                requestClients.AddRange(_requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 7));
                requestClients.AddRange(_requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 8));
            }
            else if (status == "Unpaid")
            {
                totalRequests = _requestClientRepository.countRequestClientByRequesterType(requestTypeId, status: 9);
                requestClients = _requestClientRepository.getRequestClientByRequesterType(requestTypeId, skip: skip, status: 9);
            }
            return getTableModal(requestClients, totalRequests, pageNo);
        }

        private TableModel getTableModal(List<RequestClient> requestClients, int totalRequests, int pageNo)
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
                    Notes = requestClient.Symptoms,
                    RegionId = requestClient.RegionId,
                    PhysicianName = requestClient.Physician != null? requestClient.Physician.FirstName+" "+requestClient.Physician.LastName : "",
                    RequesterType = requestClient.Request.RequestTypeId,
                    BirthDate = requestClient.IntYear != null ? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth
                                                               + "-" + requestClient.IntDate) : null,
                    RequestdDate = requestClient.Request.CreatedDate,
                    Email = requestClient.Email,
                    DateOfService = null,
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

        private DataTable convertRequestClientToDataTable(List<RequestClient> requestClients)
        {
            List<String> columnsNames = new List<String>();
            DataTable dataTable = new DataTable();
            dataTable.TableName = "RequestDatas";
            int currentRow = 1, index = 1;
            foreach (PropertyInfo propertyInfo in typeof(RequestClient).GetProperties())
            {
                dataTable.Columns.Add(propertyInfo.Name);
                columnsNames.Add(propertyInfo.Name);
                index++;
            }
            DataRow row;
            foreach (RequestClient requestClient in requestClients)
            {
                row = dataTable.NewRow();
                for (int i = 0; i < columnsNames.Count; i++)
                {
                    var value = typeof(RequestClient).GetProperty(columnsNames[i]).GetValue(requestClient);
                    if (value != null)
                    {
                        row[columnsNames[i]] = value.ToString();
                    }
                }
                dataTable.Rows.Add(row);
                currentRow++;
            }
            return dataTable;
        }

        private DataTable convertRequestClientToDataTable(List<TablesData> tablesDatas)
        {
            List<String> columnsNames = new List<String>();
            DataTable dataTable = new DataTable();
            dataTable.TableName = "RequestDatas";
            int currentRow = 1, index = 1;
            foreach (PropertyInfo propertyInfo in typeof(TablesData).GetProperties())
            {
                dataTable.Columns.Add(propertyInfo.Name);
                columnsNames.Add(propertyInfo.Name);
                index++;
            }
            DataRow row;
            foreach (TablesData tablesData in tablesDatas)
            {
                row = dataTable.NewRow();
                for (int i = 0; i < columnsNames.Count; i++)
                {
                    var value = typeof(TablesData).GetProperty(columnsNames[i]).GetValue(tablesData);
                    if (value != null)
                    {
                        row[columnsNames[i]] = value.ToString();
                    }
                }
                dataTable.Rows.Add(row);
                currentRow++;
            }
            return dataTable;
        }

        private String genrateHash(String password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashPassword).Replace("-", "").ToLower();
            }
        }
    }
}