using Microsoft.EntityFrameworkCore;
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

        public List<RequestClient> getRequestClientByStatus(int status)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Where(a => a.Status==status).ToList();
        }

        public List<RequestClient> getAllRequestClientForUser(int userId)
        {
            return _dbContext.RequestClients.Where(r => r.Request.UserId==userId).ToList();
        }

        public async Task<int> addRequestClient(RequestClient requestClient)
        {
            _dbContext.Add(requestClient);
            await _dbContext.SaveChangesAsync();
            return requestClient?.RequestClientId ?? 0;
        }

        public RequestClient GetRequestClientByRequestId(int requestId)
        {
            return _dbContext.RequestClients.FirstOrDefault(a => a.RequestId==requestId);
        }
    }
}
