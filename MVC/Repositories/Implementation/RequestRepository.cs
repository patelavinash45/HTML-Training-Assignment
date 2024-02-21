using Repositories.ViewModels;
using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementation
{
    public class RequestRepository : IRequestRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Request> getAllRequest()
        {
            return _dbContext.Requests.ToList();
        }

        public async Task<int> addRequest(int userId, AddPatientRequest model)
        {
            Request request = new()
            {
                RequestTypeId = 2,
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Mobile,
                CreatedDate = DateTime.Now,
            };
            _dbContext.Add(request);
            await _dbContext.SaveChangesAsync();
            return request?.RequestId ?? 0;
        }
    }
}
