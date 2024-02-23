using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels.Admin;
using Services.Interfaces.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation.Admin
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
            RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
            ViewCase viewCase = new() 
            { 
                FirstName = requestClient.FirstName,
                LastName = requestClient.LastName,
                BirthDate= requestClient.IntYear != null ? DateTime.Parse(requestClient.IntYear + "-" + requestClient.StrMonth
                                 + "-" + requestClient.IntDate) : null,
                Mobile=requestClient.PhoneNumber,
                Email=requestClient.Email,
                Address=requestClient.Street+ " " +requestClient.City + " " + requestClient.State + " " + requestClient.ZipCode,
                Region=requestClient.State,
            };
            return viewCase;
        }
    }
}
