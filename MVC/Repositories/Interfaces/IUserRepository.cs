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

        int addUser(AddPatientRequest model, int aspNetUserId);
    }
}
