using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class RequestClientRepository : IRequestClientRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestClientRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<RequestClient> getAllRequestClient()
        {
            return _dbContext.RequestClients.ToList();
        }

        public async Task<int> addRequestClient(int userId, AddPatientRequest model, int requestId,int regionId)
        {
            RequestClient requestClient = new()
            {
                RequestId = requestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
                RegionId = regionId,
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
            _dbContext.Add(requestClient);
            await _dbContext.SaveChangesAsync();
            return requestClient?.RequestClientId ?? 0;
        }
    }
}
