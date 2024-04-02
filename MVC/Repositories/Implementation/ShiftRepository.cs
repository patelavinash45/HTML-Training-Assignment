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

        public List<ShiftDetail> getShiftDetailByPhysicianId(int physicianId, DateTime startTime, DateTime endTime)
        {
            return _dbContext.ShiftDetails.Include(a => a.Shift).Where(a => a.Shift.PhysicianId == physicianId && 
                                                                       a.ShiftDate >= startTime && a.ShiftDate <= endTime).ToList();
        }

        public async Task<bool> addShift(Shift shift)
        {
            _dbContext.Shifts.Add(shift);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> addShiftDetails(ShiftDetail shiftDetail)
        {
            _dbContext.ShiftDetails.Add(shiftDetail);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
