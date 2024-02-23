using Repositories.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IAspNetUserRepository
    {
        int validateUser(String email,String password);

        int checkUser(String email);

        AspNetUser getUser(int aspNetUserId);

        Task<int> addUser(AspNetUser aspNetUser);

        Task<bool> setToken(String token, int aspNetUserId);

        bool checkToken(String token,int aspNetUserId);

        Task<bool> changePassword(AspNetUser aspNetUser,String password);
    }
}
