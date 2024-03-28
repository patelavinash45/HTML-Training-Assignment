using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces;
using Services.ViewModels;
using Services.ViewModels.Admin;
using System.Text.Json;

namespace Services.Implementation
{
    public class ViewProfileService : IViewProfileService
    {
        private readonly IUserRepository _userRepository;
        private IRequestClientRepository _requestClientRepository;

        public ViewProfileService(IUserRepository userRepository, IRequestClientRepository requestClientRepository)
        {
            _userRepository = userRepository;
            _requestClientRepository = requestClientRepository;
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

        public async Task<bool> updatePatientProfile(ViewProfile model, int aspnetUserId)
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

        public AdminCreaateAndProfile GetAdminViewProfile(int aspNetUserId)
        {
            Admin admin = _userRepository.getAdmionByAspNetUserId(aspNetUserId);
            List<Region> allRegion = _requestClientRepository.getAllRegions();
            Dictionary<int, string> regions = new Dictionary<int, string>();
            Dictionary<int, bool> adminRegions = new Dictionary<int, bool>();
            foreach (Region region in allRegion)
            {
                adminRegions.Add(region.RegionId,false);
                regions.Add(region.RegionId, region.Name);
            }
            List<AdminRegion> allAdminRegions = _userRepository.getAdminRegionByAdminId(admin.AdminId);
            foreach (AdminRegion adminRegion in allAdminRegions)
            {
                adminRegions[adminRegion.RegionId] = true;
            }
            AdminCreaateAndProfile adminViewProfile = new AdminCreaateAndProfile()
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                Mobile = admin.Mobile,
                Phone = admin.AltPhone,
                Status = admin.Status,
                Address1 = admin.Address1,
                Address2 = admin.Address2,
                City = admin.City,
                ZipCode = admin.Zip,
                SelectedRegion = admin.RegionId.ToString(),
                Regions = regions,
                AdminRegions = adminRegions,
            };
            return adminViewProfile;
        }

        public async Task<bool> editEditAdministratorInformastion(String data1, int aspNetUserId)
        {
            AdminCreaateAndProfile _data = JsonSerializer.Deserialize<AdminCreaateAndProfile>(data1);
            Admin admin = _userRepository.getAdmionByAspNetUserId(aspNetUserId);
            admin.FirstName = _data.FirstName; 
            admin.LastName = _data.LastName;
            admin.Email = _data.Email; 
            admin.Mobile = _data.Mobile;
            if(await _userRepository.updateAdmin(admin))
            {
                List<AdminRegion> adminRegions = _userRepository.getAdminRegionByAdminId(admin.AdminId);
                foreach(AdminRegion adminRegion in adminRegions)
                {
                    if (!_data.SelectedRegions.Contains(adminRegion.RegionId.ToString()))
                    {
                        await _userRepository.deleteAdminRgion(adminRegion);
                    }
                }
                foreach (String regionId in _data.SelectedRegions)
                {
                    if (!adminRegions.Any(a => a.RegionId == int.Parse(regionId)))
                    {
                        AdminRegion _adminRegion = new AdminRegion()
                        {
                            AdminId = admin.AdminId,
                            RegionId = int.Parse(regionId),
                        };
                        await _userRepository.addAdminRgion(_adminRegion);
                    }
                }
                return true;
            }
            return false;
        }

        public async Task<bool> editMailingAndBillingInformation(String data, int aspNetUserId)
        {
            AdminCreaateAndProfile _data = JsonSerializer.Deserialize<AdminCreaateAndProfile>(data);
            Admin admin = _userRepository.getAdmionByAspNetUserId(aspNetUserId);
            admin.Address1 = _data.Address1;
            admin.Address2 = _data.Address2;
            admin.City = _data.City;
            admin.RegionId = int.Parse(_data.SelectedRegion);
            admin.Zip = _data.ZipCode;
            admin.AltPhone = _data.Phone;
            return await _userRepository.updateAdmin(admin);
        }
    }
}
