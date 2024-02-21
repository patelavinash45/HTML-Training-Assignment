using Repositories.DataModels;
using Repositories.Implementation;
using Repositories.Interface;
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

        public AddRequestService(IAspNetRoleRepository aspNetRoleRepository,IAspNetUserRepository aspNetUserRepository,
                                    IUserRepository userRepository, IAspNetUserRoleRepository aspNetuserRoleRepository,IRequestRepository requestRepository,
                                    IRequestWiseFileRepository requestWiseFileRepository, IRegionRepository regionRepository,
                                    IRequestClientRepository requestClientRepository )
        {
            _aspNetRoleRepository = aspNetRoleRepository;
            _aspNetUserRepository = aspNetUserRepository;
            _userRepository = userRepository;
            _aspNetuserRoleRepository = aspNetuserRoleRepository;
            _requestRepository = requestRepository;
            _requestWiseFileRepository = requestWiseFileRepository;
            _regionRepository = regionRepository;
            _requestClientRepository = requestClientRepository;
        }

        public async Task<bool> addPatientRequest(AddPatientRequest model)
        {
            int aspNetRoleId = _aspNetRoleRepository.checkUserRole(role: "Patient");
            if (aspNetRoleId==0)
            {
                aspNetRoleId=await _aspNetRoleRepository.addUserRole(role: "Patient");
            }
            int aspNetUserId = _aspNetUserRepository.checkUser(email: model.Email);
            int userId = _userRepository.getUserID(aspNetUserId);
            if (aspNetUserId==0)
            {
                aspNetUserId = await _aspNetUserRepository.addUser(email:model.Email, password:model.Password,firstName: model.FirstName,mobile: model.Mobile);
                userId= await _userRepository.addUser(model: model,aspNetUserId: aspNetUserId);
                _aspNetuserRoleRepository.addAspNetUserRole(userId:userId, roleId:aspNetRoleId);
            }
            int requestId= await _requestRepository.addRequest(userId:userId, model:model);
            if(model.File!= null)
            {
                await _requestWiseFileRepository.addFile(requestId: requestId, model: model);
            }
            int regionId = _regionRepository.checkRegion(model.State);
            if (regionId==0)
            {
                regionId = await _regionRepository.addRegion(model.State);
            }
            int requestClientId = await _requestClientRepository.addRequestClient(userId:userId, model:model,requestId:requestId,regionId:regionId);
            if(requestClientId==0)
            {
                return false;
            }
            return true;
        }
    }
}
