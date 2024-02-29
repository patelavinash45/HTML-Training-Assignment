using Microsoft.EntityFrameworkCore;
using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces.Patient;
using System.Security.Cryptography;
using System.Text;

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
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IFileService _fileService;
        private readonly IConciergeRepository _conciergeRepository;
        private readonly IRequestConciergeRepository _requestConciergeRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IRequestBusinessRepository _requestBusinessRepository;

        public AddRequestService(IAspNetRoleRepository aspNetRoleRepository, IAspNetUserRepository aspNetUserRepository,
                                    IUserRepository userRepository, IAspNetUserRoleRepository aspNetuserRoleRepository, IRequestRepository requestRepository,
                                    IRequestWiseFileRepository requestWiseFileRepository, IRequestBusinessRepository requestBusinessRepository,
                                    IRequestClientRepository requestClientRepository, IFileService fileService, IConciergeRepository conciergeRepository,
                                    IRequestConciergeRepository requestConciergeRepository, IBusinessRepository businessRepository)
        {
            _aspNetRoleRepository = aspNetRoleRepository;
            _aspNetUserRepository = aspNetUserRepository;
            _userRepository = userRepository;
            _aspNetuserRoleRepository = aspNetuserRoleRepository;
            _requestRepository = requestRepository;
            _requestWiseFileRepository = requestWiseFileRepository;
            _requestClientRepository = requestClientRepository;
            _fileService = fileService;
            _conciergeRepository = conciergeRepository;
            _requestConciergeRepository = requestConciergeRepository;
            _businessRepository = businessRepository;
            _requestBusinessRepository = requestBusinessRepository;
        }

        public bool IsEmailExists(String email)
        {
            int aspNetUserId = _aspNetUserRepository.checkUser(email);
            return aspNetUserId == 0 ? false : true;
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
                    PasswordHash = genrateHash(model.Password),
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
                    UserId = aspNetUserId,
                    RoleId = aspNetRoleId,
                };
                await _aspNetuserRoleRepository.addAspNetUserRole(aspNetUserRole);
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
            RequestClient requestClient = new()
            {
                RequestId = requestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
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
            return await _requestClientRepository.addRequestClient(requestClient) == 0 ? false : true;
        }

        public AddRequestByPatient getModelForRequestByMe(int aspNetUserId)
        {
            User user = _userRepository.getUser(aspNetUserId);
            DateTime birthDay = DateTime.Parse(user.IntYear + "-" + user.StrMonth + "-" + user.IntDate);
            DashboardHeader dashboardHeader = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AspNetUserId = aspNetUserId,
            };
            AddRequestByPatient addRequestForMe = new()
            {
                AspNetUserId = aspNetUserId,
                Header = dashboardHeader,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = birthDay,
                Mobile = user.Mobile,
                Email = user.Email,
            };
            return addRequestForMe;
        }

        public async Task<bool> addRequestForMe(AddRequestByPatient model)
        {
            AddPatientRequest patientRequest = new()
            {
                Symptoms = model.Symptoms,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Mobile = model.Mobile,
                Password = model.Password,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                House = model.House,
                BirthDate = model.BirthDate,
                File = model.File,
            };
            return await addPatientRequest(patientRequest);
        }

        public AddRequestByPatient getModelForRequestForSomeoneelse(int aspNetUserId)
        {
            User user = _userRepository.getUser(aspNetUserId);
            DateTime birthDay = DateTime.Parse(user.IntYear + "-" + user.StrMonth + "-" + user.IntDate);
            DashboardHeader dashboardHeader = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AspNetUserId = aspNetUserId,
            };
            AddRequestByPatient addRequestForMe = new()
            {
               Header = dashboardHeader,
            };
            return addRequestForMe;
        }

        public async Task<bool> addRequestForSomeOneelse(AddRequestByPatient model,int aspNetUserIdMe)
        {
            int aspNetUserId = _aspNetUserRepository.checkUser(email: model.Email);
            int userId = _userRepository.getUserID(aspNetUserId);
            if (aspNetUserId == 0)
            {
                AspNetUser aspNetUser = new()
                {
                    UserName = model.FirstName,
                    Email = model.Email,
                    PhoneNumber = model.Mobile,
                    PasswordHash = genrateHash(model.Password),
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
                    UserId = aspNetUserId,
                    RoleId = _aspNetRoleRepository.checkUserRole(role: "Patient"),
                };
                await _aspNetuserRoleRepository.addAspNetUserRole(aspNetUserRole);
            }
            User userMe = _userRepository.getUser(aspNetUserIdMe);
            Request request = new()
            {
                RequestTypeId = 4,
                FirstName = userMe.FirstName,
                LastName = userMe.LastName,
                Email = userMe.Email,
                PhoneNumber = userMe.Mobile,
                UserId = userId,
            };
            int requestId = await _requestRepository.addRequest(request);
            if (model.File != null)
            {
                await _fileService.addFile(requestId: requestId, file: model.File, firstName: model.FirstName, lastName: model.LastName);
            }
            RequestClient requestClient = new()
            {
                RequestId = requestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
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
            return await _requestClientRepository.addRequestClient(requestClient) == 0 ? false : true;
        }

        public async Task<bool> addConciergeRequest(AddConciergeRequest model)
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
                    PasswordHash = genrateHash(model.Password),
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
                    UserId = aspNetUserId,
                    RoleId = aspNetRoleId,
                };
                await _aspNetuserRoleRepository.addAspNetUserRole(aspNetUserRole);
            }
            Request request = new()
            {
                RequestTypeId = 4,
                UserId = userId,
                FirstName = model.ConciergeFirstName,
                LastName = model.ConciergeLastName,
                Email = model.ConciergeEmail,
                PhoneNumber = model.ConciergeMobile,
                CreatedDate = DateTime.Now,
            };
            int requestId = await _requestRepository.addRequest(request);
            if (model.File != null)
            {
                await _fileService.addFile(requestId: requestId, file: model.File, firstName: model.FirstName, lastName: model.LastName);
            }
            Concierge concierge = new()
            {
                ConciergeName = model.ConciergeFirstName,
                Street = model.ConciergeStreet,
                City = model.ConciergeCity,
                State = model.ConciergeState,
                ZipCode = model.ConciergeZipCode,
                CreatedDate = DateTime.Now,
            };
            int conciergeId = await _conciergeRepository.addConcierge(concierge);
            RequestConcierge requestConcierge = new()
            {
                RequestId = request.RequestId,
                ConciergeId = concierge.ConciergeId
            };
            await _requestConciergeRepository.addRequestConcierge(requestConcierge);
            RequestClient requestClient = new()
            {
                RequestId = requestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
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
            return await _requestClientRepository.addRequestClient(requestClient) == 0 ? false : true;
        }

        public async Task<bool> addFamilyFriendRequest(AddFamilyRequest model)
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
                    PasswordHash = genrateHash(model.Password),
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
                    UserId = aspNetUserId,
                    RoleId = aspNetRoleId,
                };
                await _aspNetuserRoleRepository.addAspNetUserRole(aspNetUserRole);
            }
            Request request = new()
            {
                RequestTypeId = 4,
                UserId = userId,
                FirstName = model.FamilyFriendFirstName,
                LastName = model.FamilyFriendLastName,
                Email = model.FamilyFriendEmail,
                PhoneNumber = model.FamilyFriendMobile,
                CreatedDate = DateTime.Now,
            };
            int requestId = await _requestRepository.addRequest(request);
            if (model.File != null)
            {
                await _fileService.addFile(requestId: requestId, file: model.File, firstName: model.FirstName, lastName: model.LastName);
            }
            RequestClient requestClient = new()
            {
                RequestId = requestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
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
            return await _requestClientRepository.addRequestClient(requestClient) == 0 ? false : true;
        }

        public async Task<bool> addBusinessRequest(AddBusinessRequest model)
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
                    PasswordHash = genrateHash(model.Password),
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
                    UserId = aspNetUserId,
                    RoleId = aspNetRoleId,
                };
                await _aspNetuserRoleRepository.addAspNetUserRole(aspNetUserRole);
            }
            Request request = new()
            {
                RequestTypeId = 4,
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.BusinessLastName,
                Email = model.BusinessEmail,
                PhoneNumber = model.BusinessMobile,
                CreatedDate = DateTime.Now,
            };
            int requestId = await _requestRepository.addRequest(request);
            if (model.File != null)
            {
                await _fileService.addFile(requestId: requestId, file: model.File, firstName: model.FirstName, lastName: model.LastName);
            }
            Business business = new()
            {
                Name = model.BusinessFirstName,
                PhoneNumber = model.BusinessMobile,
                CreatedDate = DateTime.Now,
            };
            int businessId = await _businessRepository.addBusiness(business);
            RequestBusiness requestBusiness = new()
            {
                RequestId = request.RequestId,
                BusinessId = business.BusinessId,
            };
            await _requestBusinessRepository.addRequestBusiness(requestBusiness);
            RequestClient requestClient = new()
            {
                RequestId = requestId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Mobile,
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
            return await _requestClientRepository.addRequestClient(requestClient) == 0 ? false : true;
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
