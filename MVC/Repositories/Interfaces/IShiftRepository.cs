using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IShiftRepository
    {
        List<Physician> getPhysicianWithShiftDetailByRegionIdAndDAte(int regionId, DateTime startDate, DateTime endDate);

        List<ShiftDetail> getShiftDetailByRegionIdAndDAte(int regionId, DateTime startDate,DateTime endDate);

        Task<bool> addShift(Shift shift);

        Task<bool> addShiftDetails(ShiftDetail shiftDetail);

        Task<bool> addShiftDetailsRegion(ShiftDetailRegion shiftDetailRegion);

        Task<bool> updateShiftDetails(ShiftDetail shiftDetail);

        Task<bool> updateShiftDetailRegion(ShiftDetailRegion shiftDetailRegion);

        ShiftDetail getShiftDetails(int shiftDetailsId);

        ShiftDetail getShiftDetailsWithPhysician(int shiftDetailsId);

        ShiftDetailRegion getShiftDetailRegion(int shiftDetailsId);

        List<ShiftDetail> getAllShiftDetails(int regionId, bool isThisMonth, DateTime date, int skip);

        int countAllShiftDetails(int regionId, bool isThisMonth, DateTime date);
    }
}