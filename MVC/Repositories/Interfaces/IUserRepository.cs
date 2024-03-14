using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        int getUserID(int aspNetUserID);

        Task<int> addUser(User user);

        User getUser(int aspNetUserID);

        Task<bool> updateProfile(User user);

        Admin getAdmionByAspNetUserId(int aspNetUserId);

        List<Physician> getAllPhysiciansByRegionId(int regionId);

        Physician getPhysicianNameByPhysicianId(int physicianId);

    }
}
