using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class RequestClientRepository : IRequestClientRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestClientRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Region> getAllRegions()
        {
            return _dbContext.Regions.ToList();
        }

        public List<RequestClient> getRequestClientByStatus(int status,int skip)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Where(a => a.Status==status).Skip(skip).OrderByDescending(a => a.RequestClientId)
                                                       .Take(10).ToList();
        }

        public int countRequestClientByStatus(int status)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Where(a => a.Status == status).ToList().Count;
        }

        public List<RequestClient> getAllRequestClientForUser(int userId)
        {
            return _dbContext.RequestClients.Where(r => r.Request.UserId==userId).ToList();
        }

        public async Task<int> addRequestClient(RequestClient requestClient)
        {
            _dbContext.Add(requestClient);
            int temp = await _dbContext.SaveChangesAsync();
            return temp>0 ? requestClient.RequestClientId : 0;
        }

        public RequestClient GetRequestClientByRequestId(int requestId)
        {
            return _dbContext.RequestClients.FirstOrDefault(a => a.RequestId==requestId);
        }

        public RequestClient GetRequestClientAndRequestByRequestId(int requestId)
        {
            return _dbContext.RequestClients.Include(a => a.Request).FirstOrDefault(a => a.RequestId == requestId);
        }

        public async Task<bool> updateRequestClient(RequestClient requestClient)
        {
            _dbContext.Update(requestClient);
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0 ? true : false;
        }

        public List<RequestClient> getRequestClientByName(string firstName,string lastName)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Where(a => a.FirstName.Contains(firstName) && a.LastName.Contains(lastName)).
                                        ToList();
        }

        public List<CaseTag> getAllReason()
        {
            return _dbContext.CaseTags.ToList();
        }
    }
}
