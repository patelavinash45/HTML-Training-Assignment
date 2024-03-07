using Repositories.DataModels;
using Services.ViewModels;

namespace Services.Interfaces.AuthServices
{
    public interface ILoginService
    {
        UserDataModel auth(Login model,int userType);

    }
}
