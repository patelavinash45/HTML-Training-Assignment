using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class RequestConciergeRepository : IRequestConciergeRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RequestConciergeRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> addRequestConcierge(RequestConcierge requestConcierge)
        {
            _dbContext.Add(requestConcierge);
            await _dbContext.SaveChangesAsync();
            return requestConcierge?.ConciergeId ?? 0;
        }
    }
}
