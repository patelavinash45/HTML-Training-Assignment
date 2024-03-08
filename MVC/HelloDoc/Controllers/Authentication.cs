using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Interfaces.AuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HelloDoc.Authentication
{
    public class Authorization : Attribute, IAuthorizationFilter
    {
        private string _role;

        public Authorization(string role)
        {
            _role = role;
        }

        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    String role = context.HttpContext.Session.GetString("role");
        //    int? aspnetUserId = context.HttpContext.Session.GetInt32("aspNetUserId");
        //    if (aspnetUserId == null)
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        //        {
        //              Controller = _role,
        //              action = "WrongLogInPage",
        //        }));
        //    }
        //    else if (aspnetUserId != null && _role != role)
        //    {
        //        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
        //        {
        //            Controller = "Patient",
        //            action = "AccessDenied",
        //        }));
        //    }
        //}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IJwtService jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();
            if (jwtService != null)
            {
                String token = context.HttpContext.Request.Cookies["jwtToken"];
                if (token == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Controller = _role,
                        action = "LoginPage",
                    }));
                }
                JwtSecurityToken jwtToken = new JwtSecurityToken();
                if (!jwtService.ValidateToken(token, out jwtToken))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Controller = "Patient",
                        action = "PatientSite",
                    }));
                }
                else if (token != null)
                {
                    String role = context.HttpContext.Session.GetString("role");
                    int? aspnetUserId = context.HttpContext.Session.GetInt32("aspNetUserId");
                    string jwtRole = jwtToken.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Role).Value;
                    if (jwtRole != _role)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            Controller = "Patient",
                            action = "AccessDenied",
                        }));
                    }
                }
            }
        }

    }
}
