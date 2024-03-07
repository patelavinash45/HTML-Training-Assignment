using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Admin getAdmionByAspNetUserId(int aspNetUserId);
    }
}
