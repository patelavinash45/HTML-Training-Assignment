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

        List<Physician> getAllPhysicians();

        List<Physician> getAllPhysiciansByRegionId(int regionId);

        PhysicianNotification GetPhysicianNotification(int physicianId);

        Task<bool> updatePhysicianNotification(PhysicianNotification physicianNotification);

        Physician getPhysicianNameByPhysicianId(int physicianId);

        List<AdminRegion> getAdminRegionByAdminId(int adminId);

        Task<bool> updateAdmin(Admin admin);

        Task<bool> addAdminRgion(AdminRegion adminRegion);

        Task<bool> deleteAdminRgion(AdminRegion adminRegion);

    }
}
