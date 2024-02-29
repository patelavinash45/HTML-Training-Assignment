using Repositories.ViewModels;

namespace Services.Interfaces.Auth
{
    public interface ILoginService
    {
        int auth(Login model,int userType);
    }
}
