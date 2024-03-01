using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class ConciergeRepository : IConciergeRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public ConciergeRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> addConcierge(Concierge concierge)
        {
            _dbContext.Add(concierge);
            await _dbContext.SaveChangesAsync();
            return concierge?.ConciergeId ?? 0;
        }
    }
}
