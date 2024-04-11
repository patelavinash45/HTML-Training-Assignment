using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using System.Collections;

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

        public List<RequestClient> getRequestClientsBasedOnFilter(Func<RequestClient, bool> predicate)
        {
            return _dbContext.RequestClients.Include(a => a.Request).ThenInclude(a => a.RequestNotes).Include(a => a.Physician)
                                            .Include(a => a.Request.RequestStatusLogs).Where(predicate).ToList();
        }

        public List<RequestClient> getRequestClientByStatus(List<int> status, int skip, string patientName ,int regionId, int requesterTypeId)
        {
            Func<RequestClient, bool> predicate = a =>
            (requesterTypeId == 0 || a.Request.RequestTypeId == requesterTypeId) 
            && (regionId == 0 || a.RegionId == regionId)
            && (patientName == null || a.FirstName.ToLower().Contains(patientName) || a.LastName.ToLower().Contains(patientName))
            && (status.Contains(a.Status));
            return _dbContext.RequestClients.Include(a => a.Request).Include(a => a.Physician).Where(predicate)
                                          .OrderByDescending(a => a.RequestClientId).Skip(skip).Take(10).ToList();
        }

        public int countRequestClientByStatusAndFilter(List<int> status, string patientName, int regionId, int requesterTypeId)
        {
            Func<RequestClient, bool> predicate = a =>
            (requesterTypeId == 0 || a.Request.RequestTypeId == requesterTypeId)
            && (regionId == 0 || a.RegionId == regionId)
            && (patientName == null || a.FirstName.ToLower().Contains(patientName) || a.LastName.ToLower().Contains(patientName))
            && (status.Contains(a.Status));
            return _dbContext.RequestClients.Include(a => a.Request).Count(predicate);
        }

        public int countRequestClientByStatus(List<int> status)
        {
            return _dbContext.RequestClients.Count(a => status.Contains(a.Status));
        }

        public List<RequestClient> getAllRequestClientForUser(int userId)
        {
            return _dbContext.RequestClients.Include(a => a.Request.RequestWiseFiles.Where(a => a.IsDeleted != new BitArray(1, true)))
                                                           .Where(a => a.Request.UserId == userId).OrderByDescending(a => a.RequestClientId).ToList();
        }

        public async Task<bool> addRequestClient(RequestClient requestClient)
        {
            _dbContext.RequestClients.Add(requestClient);
            return await _dbContext.SaveChangesAsync() > 0;
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

        public List<RequestClient> getAllRequestClientsByUserId(int userId, int skip)
        {
            return _dbContext.RequestClients.Include(a => a.Request).ThenInclude(a => a.RequestWiseFiles.Where(a => a.IsDeleted != new BitArray(1, true)))
                .Include(a => a.Physician).Where(a => a.Request.UserId == userId).OrderByDescending(a => a.RequestClientId).Skip(skip).Take(5).ToList();
        }

        public int countRequestClientsByUserId(int userId)
        {
            return _dbContext.RequestClients.Include(a => a.Request).Where(a => a.Request.UserId == userId).Count();
        }
    }
}