using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Services.Interfaces.AuthServices;
using Services.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation.AuthServices
{
    public class LoginService : ILoginService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IAspNetUserRoleRepository _aspNetUserRoleRepository;
        private readonly IUserRepository _userRepository;

        public LoginService(IAdminRepository adminRepository, IAspNetUserRoleRepository aspNetUserRoleRepository,
                                       IUserRepository userRepository)
        {
            _adminRepository = adminRepository;
            _aspNetUserRoleRepository = aspNetUserRoleRepository;
            _userRepository = userRepository;
        }

        public UserDataModel auth(Login model,int userType)
        {
            AspNetUserRole aspNetUserRole = _aspNetUserRoleRepository.
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
                    Admin admin = _adminRepository.getAdmionByAspNetUserId(aspNetUserRole.UserId);
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

