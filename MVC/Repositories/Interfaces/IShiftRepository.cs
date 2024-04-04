using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IShiftRepository
    {
        List<ShiftDetail> getShiftDetailByPhysicianId(int physicianId,DateTime startDate,DateTime endDate);

        List<ShiftDetail> getAllShiftDetailsForSecificMonths(DateTime startDate, DateTime endDate);

        Task<bool> addShift(Shift shift);

        Task<bool> addShiftDetails(ShiftDetail shiftDetail);

        Task<bool> updateShiftDetails(ShiftDetail shiftDetail);

        ShiftDetail getShiftDetails(int shiftDetailsId);

        List<ShiftDetail> getAllShiftDetails(int regionId, bool isThisMonth, DateTime date, int skip);

        int countAllShiftDetails(int regionId, bool isThisMonth, DateTime date);
    }
}