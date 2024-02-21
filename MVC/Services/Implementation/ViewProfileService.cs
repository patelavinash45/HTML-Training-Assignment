using HelloDoc.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.DataModels;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ViewProfileService :IViewProfileService
    {
        private readonly IUserRepository _userRepository;

        public ViewProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public ViewProfile getProfileDetails(int aspNetUserId)
        {
            User user= _userRepository.GetUser(aspNetUserId);
            DateTime birthDay = DateTime.Parse(user.IntYear + "-" + user.StrMonth + "-" + user.IntDate);
            DashboardHeader dashboardHeader = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AspNetUserId = aspNetUserId,
            };
            ViewProfile viewProfile = new()
            {
                AspNetUserId = aspNetUserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = birthDay,
                Email = user.Email,
                Mobile = user.Mobile,
                State = user.State,
                Street = user.Street,
                City = user.City,
                ZipCode = user.ZipCode,
                Header = dashboardHeader,
            };
            return viewProfile;
        }

        public async Task<bool> updatePatientProfile(ViewProfile model)
        {
            return await _userRepository.updateProfile(model);
        }
    }
}
