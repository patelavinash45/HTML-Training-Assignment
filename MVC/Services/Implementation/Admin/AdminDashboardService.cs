using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels.Admin;
using Services.Interfaces.Admin;

namespace Services.Implementation.Admin
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IRequestClientRepository _requestClientRepository;

        public AdminDashboardService(IRequestRepository requestRepository,
                                    IRequestClientRepository requestClientRepository)
        { 
            //_requestRepository = requestRepository;
            _requestClientRepository = requestClientRepository;
        }

        public AdminDashboard getallRequests()
        {
            AdminDashboard adminDashboard = new()
            {
                NewRequests = GetNewRequest("New"),
                NewRequestCount= _requestClientRepository.getRequestClientByStatus(1).Count,
                PendingRequestCount = _requestClientRepository.getRequestClientByStatus(2).Count,
                ActiveRequestCount = _requestClientRepository.getRequestClientByStatus(4).Count + 
                                     _requestClientRepository.getRequestClientByStatus(5).Count,
                ConcludeRequestCount = _requestClientRepository.getRequestClientByStatus(6).Count,
                TocloseRequestCount = _requestClientRepository.getRequestClientByStatus(3).Count +
                                      _requestClientRepository.getRequestClientByStatus(7).Count +
                                      _requestClientRepository.getRequestClientByStatus(8).Count,
                UnpaidRequestCount = _requestClientRepository.getRequestClientByStatus(9).Count,
            };
            return adminDashboard;
        }

        public List<NewTables> GetNewRequest(String status)
        {
            List<RequestClient> requestClients= new List<RequestClient>();
            if (status=="New")
            {
                requestClients = _requestClientRepository.getRequestClientByStatus(1);
            }
            else if(status == "Pending")
            {
                requestClients = _requestClientRepository.getRequestClientByStatus(2);
            }
            else if (status == "Active")
            {
                requestClients = _requestClientRepository.getRequestClientByStatus(4);
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(5));
            }
            else if (status == "Conclude")
            {
                requestClients = _requestClientRepository.getRequestClientByStatus(6);
            }
            else if (status == "Close")
            {
                requestClients = _requestClientRepository.getRequestClientByStatus(3);
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(7));
                requestClients.AddRange(_requestClientRepository.getRequestClientByStatus(8));
            }
            else if (status == "Unpaid")
            {
                requestClients = _requestClientRepository.getRequestClientByStatus(9);
            }
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
            List<NewTables> newTables = new List<NewTables>();
            foreach(RequestClient requestClient in requestClients)
            {
                NewTables newTable = new()
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
                    RequesterType= requestClient.Request.RequestTypeId,
                    BirthDate= requestClient.IntYear!=null? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth 
                                 + "-" + requestClient.IntDate): null,
                    Email= requestClient.Email,
                    DateOfService=null,
                    PhysicianName="",
                };
                newTables.Add(newTable);
            }
            return newTables;
        }
    }
}
