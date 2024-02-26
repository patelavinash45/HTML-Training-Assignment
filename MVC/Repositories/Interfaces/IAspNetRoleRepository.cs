using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IAspNetRoleRepository
    {
        int checkUserRole(string role);

        Task<int> addUserRole(AspNetRole aspNetRole);
    }
}
