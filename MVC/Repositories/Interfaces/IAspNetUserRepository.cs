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

        bool checkUser(String email);

        int addUser(String email,String password,String firstName, String mobile);
    }
}
