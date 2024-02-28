using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class RequestStatusLogRepository : IRequestStatusLogRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestStatusLogRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RequestStatusLog GetRequestStatusLogByRequestId(int requestId)
        {
            return _dbContext.RequestStatusLogs.FirstOrDefault(a => a.RequestId == requestId);
        }

        public async Task<int> addRequestSatatusLog(RequestStatusLog requestStatusLog)
        {
            _dbContext.Add(requestStatusLog);
            await _dbContext.SaveChangesAsync();
            return requestStatusLog?.RequestStatusLogId ?? 0;
        }  

        public async Task<bool> updateRequestSatatusLog(RequestStatusLog requestStatusLog)
        {
            _dbContext.Update(requestStatusLog);
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0;
        }
    }
}
