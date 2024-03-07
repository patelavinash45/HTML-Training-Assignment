using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IAspNetUserRoleRepository
    {
        Task<bool> addAspNetUserRole(AspNetUserRole aspNetUserRole);

        AspNetUserRole validateAspNetUserRole(string email,string password,int userType);
    }
}
