using Repositories.DataModels;
using Repositories.ViewModels;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        int getUserID(int aspNetUserID);

        Task<int> addUser(User user);

        User GetUser(int aspNetUserID);

        Task<bool> updateProfile(ViewProfile model);
    }
}
