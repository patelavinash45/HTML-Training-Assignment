using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IAspNetUserRoleRepository
    {
        Task<bool> addAspNetUserRole(AspNetUserRole aspNetUserRole);

        bool validateAspNetUserRole(int aspNetUserId,int userType);
    }
}
