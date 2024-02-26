using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IAspNetUserRoleRepository
    {
        Task<bool> addAspNetUserRole(AspNetUserRole aspNetUserRole);
    }
}
