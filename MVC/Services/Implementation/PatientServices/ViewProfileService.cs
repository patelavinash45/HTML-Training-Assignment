using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.PatientServices;
using Services.ViewModels;

namespace Services.Implementation.PatientServices
{
    public class ViewProfileService : IViewProfileService
    {
        private readonly IUserRepository _userRepository;

        public ViewProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ViewProfile getProfileDetails(int aspNetUserId)
        {
            User user = _userRepository.getUser(aspNetUserId);
            DateTime birthDay = DateTime.Parse(user.IntYear + "-" + user.StrMonth + "-" + user.IntDate);
            ViewProfile viewProfile = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = birthDay,
                Email = user.Email,
                Mobile = user.Mobile,
                State = user.State,
                Street = user.Street,
                City = user.City,
                ZipCode = user.ZipCode,
                IsMobile = user.IsMobile[0] ? 1 : 0,
            };
            return viewProfile;
        }

        public async Task<bool> updatePatientProfile(ViewProfile model,int aspnetUserId)
        {
            User user = _userRepository.getUser(aspnetUserId);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Street = model.Street;
            user.City = model.City;
            user.State = model.State;
            user.IsMobile[0] = model.IsMobile == 1 ? true : false;
            user.ZipCode = model.ZipCode;
            user.IntDate = model.BirthDate.Value.Day;
            user.StrMonth = model.BirthDate.Value.Month.ToString();
            user.IntYear = model.BirthDate.Value.Year;
            return await _userRepository.updateProfile(user);
        }
    }
}
