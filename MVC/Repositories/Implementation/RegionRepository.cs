using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class RegionRepository : IRegionRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RegionRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int checkRegion(String regionName)
        {
            Region region = _dbContext.Regions.FirstOrDefault(a =>a.Name == regionName.Trim());
            return region?.RegionId ?? 0;
        }

        public async Task<int> addRegion(Region region)
        {
            _dbContext.Add(region);
            await _dbContext.SaveChangesAsync();
            return region?.RegionId ?? 0;
        }
    }
}
