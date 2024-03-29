﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Interfaces.AuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
                JwtSecurityToken jwtToken = new JwtSecurityToken();
                if (token == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Controller = _role,
                        action = "LoginPage",
                    }));
                }
                else if (token != null && !jwtService.validateToken(token, out jwtToken))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        Controller = "Patient",
                        action = "PatientSite",
                    }));
                }
                else
                {
                    String jwtRole = jwtToken.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Role).Value;
                    if (jwtRole != _role)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                        {
                            Controller = _role,
                            action = "AccessDenied",
                        }));
                    }
                    else 
                    {
                        if (context.HttpContext.Session.GetInt32("aspNetUserId") == null)
                        {
                            int jwtId = int.Parse(jwtToken.Claims.FirstOrDefault(a => a.Type == "aspNetUserId").Value);
                            String jwtFirstName = jwtToken.Claims.FirstOrDefault(a => a.Type == "firstName").Value;
                            String jwtLastName = jwtToken.Claims.FirstOrDefault(a => a.Type == "lastName").Value;
                            context.HttpContext.Session.SetString("role", jwtRole);
                            context.HttpContext.Session.SetString("firstName", jwtFirstName);
                            context.HttpContext.Session.SetString("lastName", jwtLastName);
                            context.HttpContext.Session.SetInt32("aspNetUserId", jwtId);
                        }
                    }
                }
            }
        }

    }
}
