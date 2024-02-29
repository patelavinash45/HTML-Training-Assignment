using Repositories.Interface;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces.Auth;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation.Auth
{
    public class LoginService : ILoginService
    {
        private readonly IAspNetUserRepository _aspNetUserRepository;
        private readonly IAspNetUserRoleRepository _aspNetUserRoleRepository;

        public LoginService(IAspNetUserRepository aspNetUserRepository, IAspNetUserRoleRepository aspNetUserRoleRepository)
        {
            _aspNetUserRepository = aspNetUserRepository;
            _aspNetUserRoleRepository = aspNetUserRoleRepository;
        }

        public int auth(Login model,int userType)
        {
            int aspNetUserId = _aspNetUserRepository.validateUser(email: model.Email.Trim(), password: genrateHash(model.PasswordHash));
            if (aspNetUserId > 0)
            {
                return _aspNetUserRoleRepository.validateAspNetUserRole(aspNetUserId: aspNetUserId, userType: userType) ? aspNetUserId : 0;
            }
            return 0;
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

