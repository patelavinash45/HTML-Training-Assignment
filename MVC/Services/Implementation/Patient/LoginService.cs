using Repositories.Interface;
using Repositories.ViewModels;
using Services.Interfaces.Patient;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation.Patient
{
    public class LoginService : ILoginService
    {
        private readonly IAspNetUserRepository _userRepository;

        public LoginService(IAspNetUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int auth(PatientLogin model)
        {
            return _userRepository.validateUser(email: model.Email.Trim(), password: genrateHash(model.PasswordHash));
        }

        private String genrateHash(String password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashPassword).Replace("-", "").ToLower();
            }
        }
    }
}

