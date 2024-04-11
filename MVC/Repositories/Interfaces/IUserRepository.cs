using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        int countUsers(Func<User, bool> predicat);

        List<User> getAllUser(Func<User, bool> predicat,int skip);

        int getUserID(int aspNetUserID);

        Task<int> addUser(User user);

        User getUser(int aspNetUserID);

        Task<bool> updateProfile(User user);

        Admin getAdmionByAspNetUserId(int aspNetUserId);

        Task<bool> addAdmin(Admin admin);

        List<Physician> getAllPhysicians();

        List<Physician> getAllPhysiciansByRegionId(int regionId);

        List<PhysicianRegion> getAllPhysicianRegionsByRegionId(int regionId);

        List<PhysicianRegion> getAllPhysicianRegionsByPhysicianId(int physicianId);

        List<Physician> getAllUnAssignedPhysician();

        PhysicianNotification GetPhysicianNotification(int physicianId);

        Task<bool> updatePhysicianNotification(PhysicianNotification physicianNotification);

        Physician getPhysicianByPhysicianId(int physicianId);

        List<AdminRegion> getAdminRegionByAdminId(int adminId);

        Task<bool> updateAdmin(Admin admin);

        Task<bool> addAdminRgion(AdminRegion adminRegion);

        Task<bool> deleteAdminRgion(AdminRegion adminRegion);

        Task<bool> addPhysicianRegion(PhysicianRegion physicianRegion);

        Task<bool> addPhysician(Physician physician);

        Task<bool> updatePhysician(Physician physician);

        Task<bool> addPhysicianNotification(PhysicianNotification physicianNotification);

        List<PhysicianLocation> getAllProviderLocation();

        List<Physician> getAllPhysicianWithCurrentShift(int regionId);

    }
}
