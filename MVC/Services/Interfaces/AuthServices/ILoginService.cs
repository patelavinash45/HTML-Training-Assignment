using Repositories.ViewModels;

namespace Services.Interfaces.AuthServices
{
    public interface ILoginService
    {
        int auth(Login model,int userType);
    }
}
