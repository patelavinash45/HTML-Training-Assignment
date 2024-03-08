using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Repositories.DataModels;
using Services.ViewModels;

namespace Services.Interfaces.AuthServices
{
    public interface ILoginService
    {
        UserDataModel auth(Login model,int userType);

        bool isTokenValid(HttpContext httpContext,String userType);
    }
}
