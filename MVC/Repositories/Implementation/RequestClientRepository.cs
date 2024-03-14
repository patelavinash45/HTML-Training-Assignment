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

        public List<RequestClient> getRequestClientByStatus(int status, int skip)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Include(a => a.Physician).Where(a => a.Status == status).Skip(skip).OrderByDescending(a => a.RequestClientId)
                                                       .Take(10).ToList();
        }

        public int countRequestClientByStatus(int status)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Include(a => a.Physician).Where(a => a.Status == status).ToList().Count;
        }

        public List<RequestClient> getAllRequestClientForUser(int userId)
        {
            return _dbContext.RequestClients.Where(r => r.Request.UserId == userId).ToList();
        }

        public async Task<int> addRequestClient(RequestClient requestClient)
        {
            _dbContext.RequestClients.Add(requestClient);
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0 ? requestClient.RequestClientId : 0;
        }

        public RequestClient getRequestClientByRequestId(int requestId)
        {
            return _dbContext.RequestClients.FirstOrDefault(a => a.RequestId == requestId);
        }

        public RequestClient getRequestClientAndRequestByRequestId(int requestId)
        {
            return _dbContext.RequestClients.Include(a => a.Request).FirstOrDefault(a => a.RequestId == requestId);
        }

        public async Task<bool> updateRequestClient(RequestClient requestClient)
        {
            _dbContext.RequestClients.Update(requestClient);
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0 ? true : false;
        }

        public List<RequestClient> getRequestClientByName(string firstName, string lastName, int status, int skip)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Include(a => a.Physician)
                .Where(a => a.Status == status).Where(a => (a.FirstName.ToLower().Contains(firstName) && a.LastName.ToLower().Contains(lastName))
                || (a.FirstName.ToLower().Contains(lastName) && a.LastName.ToLower().Contains(firstName))).
                Skip(skip).OrderByDescending(a => a.RequestClientId).Take(10).ToList();
        }

        public int countRequestClientByName(string firstName, string lastName, int status)
        {
            return _dbContext.RequestClients
              .Where(a => a.Status == status).Where(a => (a.FirstName.ToLower().Contains(firstName) && a.LastName.ToLower().Contains(lastName))
                || (a.FirstName.ToLower().Contains(lastName) && a.LastName.ToLower().Contains(firstName))).ToList().Count;
        }

        public List<RequestClient> getRequestClientByRegion(int regionId, int status, int skip)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Include(a => a.Physician).Where(a => a.Status == status)
                .Where(a => a.RegionId == regionId).Skip(skip).OrderByDescending(a => a.RequestClientId).Take(10).ToList();
        }

        public int countRequestClientByRegion(int regionId, int status)
        {
            return _dbContext.RequestClients.Where(a => a.Status == status).Where(a => a.RegionId == regionId).ToList().Count;
        }

        public List<RequestClient> getRequestClientByRequesterType(int requestTypeId, int status, int skip)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Include(a => a.Physician)
                .Where(a => a.Status == status).Where(a => a.Request.RequestTypeId == requestTypeId).
                        Skip(skip).OrderByDescending(a => a.RequestClientId).Take(10).ToList();
        }

        public int countRequestClientByRequesterType(int requestTypeId, int status)
        {
            return _dbContext.RequestClients.Where(a => a.Status == status).Where(a => a.Request.RequestTypeId == requestTypeId).ToList().Count;
        }

        public List<CaseTag> getAllReason()
        {
            return _dbContext.CaseTags.ToList();
        }
    }
}