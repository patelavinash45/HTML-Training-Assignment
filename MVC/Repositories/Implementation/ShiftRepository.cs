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

        public List<ShiftDetail> getShiftDetailByPhysicianId(int physicianId, DateTime startDate, DateTime endDate)
        {
            return _dbContext.ShiftDetails.Include(a => a.Shift).Where(a => a.Shift.PhysicianId == physicianId && 
                                                                  a.ShiftDate.Date >= startDate.Date && a.ShiftDate.Date <= endDate.Date).ToList();
        }

        public List<ShiftDetail> getAllShiftDetailsForSecificMonths(DateTime startDate, DateTime endDate)
        {
            return _dbContext.ShiftDetails.Include(a => a.Shift).ThenInclude(a => a.Physician)
                                                  .Where(a => a.ShiftDate.Date >= startDate.Date && a.ShiftDate.Date <= endDate.Date).ToList();
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

        public async Task<bool> updateShiftDetails(ShiftDetail shiftDetail)
        {
            _dbContext.ShiftDetails.Update(shiftDetail);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public ShiftDetail getShiftDetails(int shiftDetailsId)
        {
            return _dbContext.ShiftDetails.FirstOrDefault(a => a.ShiftDetailId == shiftDetailsId);
        }

        public List<ShiftDetail> getAllShiftDetails(int regionId, bool isThisMonth, DateTime date, int skip)
        {
            Func<ShiftDetail,bool>  predicate = a => 
            (!isThisMonth || (a.ShiftDate.Date >= date.Date && a.ShiftDate.Date <= date.AddMonths(1).Date)) 
            && (regionId == 0 || a.RegionId == regionId);
            return _dbContext.ShiftDetails.Include(a => a.Shift).ThenInclude(a => a.Physician).Include(a => a.Region)
                    .Where(predicate).OrderByDescending(a => a.ShiftDate).Skip(skip).Take(10).ToList();
        }

        public int countAllShiftDetails(int regionId, bool isThisMonth, DateTime date)
        {
            Func<ShiftDetail, bool> predicate = a =>
            (!isThisMonth || (a.ShiftDate.Date >= date.Date && a.ShiftDate.Date <= date.AddMonths(1).Date))
            && (regionId == 0 || a.RegionId == regionId);
            return _dbContext.ShiftDetails.Include(a => a.Region).Where(predicate).Count();
        }
    }
}
