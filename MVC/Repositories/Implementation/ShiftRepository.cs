using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;

namespace Repositories.Implementation
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public ShiftRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ShiftDetail> getShiftDetailByPhysicianId(int physicianId)
        {
            return _dbContext.ShiftDetails.Include(a => a.Shift).Where(a => a.Shift.PhysicianId == physicianId).ToList();
                      //.ToDictionary(shiftDetail => shiftDetail.StartTime.ToString(), shiftDetail => shiftDetail.EndTime.ToString());
        }
    }
}
