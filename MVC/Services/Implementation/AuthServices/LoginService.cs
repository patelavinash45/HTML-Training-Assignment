using Microsoft.AspNetCore.Http;
using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Services.Interfaces.AuthServices;
using Services.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation.AuthServices
{
    public class LoginService : ILoginService
    {
        private readonly IAspRepository _aspRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginService(IAspRepository aspRepository,
                                       IUserRepository userRepository,IJwtService jwtService)
        {
            _aspRepository = aspRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public UserDataModel auth(Login model,int userType)
        {
            AspNetUserRole aspNetUserRole = _aspRepository.
                           validateAspNetUserRole(email: model.Email, password: genrateHash(model.PasswordHash), userType: userType);
            if (aspNetUserRole != null)
            {
                if (userType == 1)
                {
                    User user = _userRepository.getUser(aspNetUserRole.UserId);
                    UserDataModel userDataModel = new UserDataModel()
                    {
                        AspNetUserId = aspNetUserRole.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserId = user.UserId,
                        UserType = aspNetUserRole.Role.Name,
                    };
                    return userDataModel;
                }
                else
                {
                    Admin admin = _userRepository.getAdmionByAspNetUserId(aspNetUserRole.UserId);
                    UserDataModel userDataModel = new UserDataModel()
                    {
                        AspNetUserId = aspNetUserRole.UserId,
                        FirstName = admin.FirstName,
                        LastName = admin.LastName,
                        AdminId = admin.AdminId,
                        UserType = aspNetUserRole.Role.Name,
                    };
                    return userDataModel;
                }
            }
            return null;
        }

        public bool isTokenValid(HttpContext httpContext, String userType)
        {
            String token = httpContext.Request.Cookies["jwtToken"];
            if (token != null)
            {
                JwtSecurityToken jwtToken = new JwtSecurityToken();
                if (_jwtService.validateToken(token, out jwtToken))
                {
                    var jwtRole = jwtToken.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Role);
                    if(jwtRole.Value == userType)
                    {
                        var jwtId = jwtToken.Claims.FirstOrDefault(a => a.Type == "aspNetUserId");
                        var jwtFirstName = jwtToken.Claims.FirstOrDefault(a => a.Type == "firstName");
                        var jwtLastName = jwtToken.Claims.FirstOrDefault(a => a.Type == "lastName");
                        httpContext.Session.SetString("role", jwtRole.Value);
                        httpContext.Session.SetString("firstName", jwtFirstName.Value);
                        httpContext.Session.SetString("lastName", jwtLastName.Value);
                        httpContext.Session.SetInt32("aspNetUserId", int.Parse(jwtId.Value));
                        return true;
                    }
                }
            }
            return false;
        }

        private string genrateHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashPassword).Replace("-", "").ToLower();
            }
        }
    }
}

