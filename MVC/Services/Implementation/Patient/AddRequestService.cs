using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces.Patient;

namespace Services.Implementation.Patient
{
    public class AddRequestService : IAddRequestService
    {
        private readonly IAspNetRoleRepository _aspNetRoleRepository;
        private readonly IAspNetUserRepository _aspNetUserRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAspNetUserRoleRepository _aspNetuserRoleRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IRequestWiseFileRepository _requestWiseFileRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IFileService _fileService;

        public AddRequestService(IAspNetRoleRepository aspNetRoleRepository, IAspNetUserRepository aspNetUserRepository,
                                    IUserRepository userRepository, IAspNetUserRoleRepository aspNetuserRoleRepository, IRequestRepository requestRepository,
                                    IRequestWiseFileRepository requestWiseFileRepository, IRegionRepository regionRepository,
                                    IRequestClientRepository requestClientRepository, IFileService fileService)
        {
            _aspNetRoleRepository = aspNetRoleRepository;
            _aspNetUserRepository = aspNetUserRepository;
            _userRepository = userRepository;
            _aspNetuserRoleRepository = aspNetuserRoleRepository;
            _requestRepository = requestRepository;
            _requestWiseFileRepository = requestWiseFileRepository;
            _regionRepository = regionRepository;
            _requestClientRepository = requestClientRepository;
            _fileService = fileService;
        }

        public async Task<bool> addPatientRequest(AddPatientRequest model)
        {
            int aspNetRoleId = _aspNetRoleRepository.checkUserRole(role: "Patient");
            if (aspNetRoleId == 0)
            {
                AspNetRole aspNetRole = new()
                {
                    Name = "Patient",
                };
                aspNetRoleId = await _aspNetRoleRepository.addUserRole(aspNetRole);
            }
            int aspNetUserId = _aspNetUserRepository.checkUser(email: model.Email);
            int userId = _userRepository.getUserID(aspNetUserId);
            if (aspNetUserId == 0)
            {
                AspNetUser aspNetUser = new()
                {
                    UserName = model.FirstName,
                    Email = model.Email,
                    PhoneNumber = model.Mobile,
                    PasswordHash = model.Password,
                    CreatedDate = DateTime.Now,
                };
                aspNetUserId = await _aspNetUserRepository.addUser(aspNetUser);
                User user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    AspNetUserId = aspNetUserId,
                    CreatedBy = aspNetUserId,
                    CreatedDate = DateTime.Now,
                    House = model.House,
                    IntYear = model.BirthDate.Value.Year,
                    IntDate = model.BirthDate.Value.Day,
                    StrMonth = model.BirthDate.Value.Month.ToString(),
                };
                userId = await _userRepository.addUser(user);
                AspNetUserRole aspNetUserRole = new()
                {
                    UserId = userId,
                    RoleId = aspNetRoleId,
                };
                _aspNetuserRoleRepository.addAspNetUserRole(aspNetUserRole);
            }
            Request request = new()
            {
                RequestTypeId = 2,
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Mobile,
                CreatedDate = DateTime.Now,
            };
            int requestId = await _requestRepository.addRequest(request);
            if (model.File != null)
            {
                await _fileService.addFile(requestId: requestId, file: model.File, firstName: model.FirstName, lastName:model.LastName);
            }
            int regionId = _regionRepository.checkRegion(model.State);
            if (regionId == 0)
            {
                Region region = new()
                {
                    Name = model.State,
                };
                regionId = await _regionRepository.addRegion(region);
            }
            RequestClient requestClient = new()
            {
                RequestId = requestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
                RegionId = regionId,
                Email = model.Email,
                State = model.State,
                Street = model.Street,
                City = model.City,
                ZipCode = model.ZipCode,
                Status = 1,
                Symptoms = model.Symptoms,
                IntYear = DateTime.Now.Year,
                IntDate = DateTime.Now.Day,
                StrMonth = DateTime.Now.Month.ToString(),
            };
            int requestClientId = await _requestClientRepository.addRequestClient(requestClient);
            if (requestClientId == 0)
            {
                return false;
            }
            return true;
        }
    }
}
