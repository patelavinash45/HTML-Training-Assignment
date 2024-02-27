using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class RequestSatatusLogRepository : IRequestSatatusLogRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestSatatusLogRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RequestStatusLog GetRequestStatusLogByRequestId(int requestId)
        {
            return _dbContext.RequestStatusLogs.FirstOrDefault(a => a.RequestId == requestId);
        }
    }
}
