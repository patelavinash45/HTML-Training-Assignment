using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRegionRepository
    {
        int checkRegion(String regionName);

        Task<int> addRegion(Region region);

        List<Region> getRegions();
    }
}
