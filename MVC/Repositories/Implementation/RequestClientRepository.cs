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

        public List<RequestClient> getAllRequestClients()
        {
            return _dbContext.RequestClients.Include(a => a.Request) .ToList();
        }

        public List<RequestClient> getRequestClientByStatus(List<int> status, int skip, string patientName ,int regionId, int requesterTypeId)
        {
            Func<RequestClient, bool> predicate = a =>
            (requesterTypeId == 0 || a.Request.RequestTypeId == requesterTypeId) 
            && (regionId == 0 || a.RegionId == regionId)
            && (patientName == null || a.FirstName.ToLower().Contains(patientName) || a.LastName.ToLower().Contains(patientName))
            && (a.Status == status[0] || a.Status == status[1] || a.Status == status[2]) ;
            return _dbContext.RequestClients.Include(a => a.Request).Include(a => a.Physician).Where(predicate)
                                          .OrderByDescending(a => a.RequestClientId).Skip(skip).Take(10).ToList();
        }

        public int countRequestClientByStatusAndFilter(List<int> status, string patientName, int regionId, int requesterTypeId)
        {
            Func<RequestClient, bool> predicate = a =>
            (requesterTypeId == 0 || a.Request.RequestTypeId == requesterTypeId)
            && (regionId == 0 || a.RegionId == regionId)
            && (patientName == null || a.FirstName.ToLower().Contains(patientName) || a.LastName.ToLower().Contains(patientName))
            && (a.Status == status[0] || a.Status == status[1] || a.Status == status[2]);
            return _dbContext.RequestClients.Include(a => a.Request).Where(predicate).ToList().Count;
        }

        public int countRequestClientByStatus(List<int> status)
        {
            return _dbContext.RequestClients.Where(a => (a.Status == status[0] || a.Status == status[1] || a.Status == status[2])).ToList().Count;
        }

        public List<RequestClient> getAllRequestClientForUser(int userId)
        {
            return _dbContext.RequestClients.Where(r => r.Request.UserId == userId).OrderByDescending(a => a.RequestClientId).ToList();
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
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public List<CaseTag> getAllReason()
        {
            return _dbContext.CaseTags.ToList();
        }
    }
}