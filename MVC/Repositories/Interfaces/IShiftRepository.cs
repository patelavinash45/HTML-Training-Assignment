using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IShiftRepository
    {
        List<ShiftDetail> getShiftDetailByPhysicianId(int physicianId);
    }
}