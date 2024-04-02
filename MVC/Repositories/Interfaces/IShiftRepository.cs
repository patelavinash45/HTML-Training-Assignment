using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IShiftRepository
    {
        List<ShiftDetail> getShiftDetailByPhysicianId(int physicianId,DateTime startTime,DateTime endTime);

        Task<bool> addShift(Shift shift);

        Task<bool> addShiftDetails(ShiftDetail shiftDetail);
    }
}