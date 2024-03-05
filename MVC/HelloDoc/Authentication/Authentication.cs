using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HelloDoc.Authentication
{
    public class Authentication : Attribute , IAuthorizationFilter
    {
        private HttpContextAccessor _contextAccessor;
        private string _role;

        public Authentication(HttpContextAccessor contextAccessor, string role)
        {
            _contextAccessor = contextAccessor;
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            String role = _contextAccessor.HttpContext.Session.GetString("Role");
            if (_contextAccessor.HttpContext.Session.GetInt32("aspNetUserId").Value == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    Controller = role,
                    action = "Dashboard",
                }));
            }
            else if(_role==role)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    Controller = "Patient",
                    action = "PatientSite",
                }));
            }
            else
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    Controller = role,
                    action = "LoginPage",
                }));
            }
        }
    }

    //public class CheckSession
    //{
    //    private HttpContextAccessor _contextAccessor;

    //    public CheckSession(HttpContextAccessor contextAccessor)
    //    {
    //        _contextAccessor = contextAccessor;
    //    }

    //    public bool checkSession()
    //    {
    //        int aspNetUserId = _contextAccessor.HttpContext.Session.GetInt32("aspNetUserId").Value;
    //    }
    //}
}
