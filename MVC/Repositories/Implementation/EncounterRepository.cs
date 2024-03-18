using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class EncounterRepository : IEncounterRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public EncounterRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Encounter getEncounter(int requestId)
        {
            return _dbContext.Encounters.FirstOrDefault(a => a.RequestId == requestId);
        }

        public async Task<bool> addEncounter(Encounter encounter)
        {
            _dbContext.Encounters.Add(encounter);
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0;
        }

        public async Task<bool> updateEncounter(Encounter encounter)
        {
            _dbContext.Encounters.Update(encounter);
            int temp = await _dbContext.SaveChangesAsync();
            return temp > 0;
        }
    }
}
