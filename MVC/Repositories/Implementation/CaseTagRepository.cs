using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class CaseTagRepository : ICaseTagRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public CaseTagRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CaseTag> getAllReason()
        {
            return _dbContext.CaseTags.ToList();
        }
    }
}
