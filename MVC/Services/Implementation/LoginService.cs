using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interface;
using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Services.Implementation
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
            return  _userRepository.validateUser(email: model.Email.Trim(), password: model.PasswordHash);
        }
    }
}

