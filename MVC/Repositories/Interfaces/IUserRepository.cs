using Repositories.DataModels;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
