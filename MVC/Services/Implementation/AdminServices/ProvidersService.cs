using Microsoft.AspNetCore.Http;
using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels.Admin;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation.AdminServices
{
    public class ProvidersService : IProvidersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAspRepository _aspRepository;

        public ProvidersService(IUserRepository userRepository, IRequestClientRepository requestClientRepository, IRoleRepository roleRepository,
                                 IAspRepository aspRepository)
        {
            _userRepository = userRepository;
            _requestClientRepository = requestClientRepository;
            _roleRepository = roleRepository;
            _aspRepository = aspRepository;
        }

        public Provider getProviders(int regionId)
        {
            Dictionary<int, string> regions = new Dictionary<int, string>();
            List<Physician> physicians = new List<Physician> { };
            if(regionId == 0)  /// by default it is 0 
            {
                physicians = _userRepository.getAllPhysicians();
                List<Region> allRegion = _requestClientRepository.getAllRegions();
                foreach (Region region in allRegion)
                {
                    regions.Add(region.RegionId, region.Name);
                }
            }
            else  /// filter
            {
                physicians = _userRepository.getAllPhysiciansByRegionId(regionId);
            }
            List<ProviderTable> providerTables = new List<ProviderTable>();
            foreach (Physician physician in physicians)
            {
                ProviderTable providerTable = new ProviderTable()
                {
                    FirstName = physician.FirstName,
                    LastName = physician.LastName,
                    Notification = physician.PhysicianNotifications.FirstOrDefault().IsNotificationStopped[0],
                    providerId = physician.PhysicianId,
                    Status = physician.Status == 1 ? "Active" : "Pending",
                };
                providerTables.Add(providerTable);
            }
            Provider provider = new Provider()
            {
                providers = providerTables,
                Regions = regions,
            };
            return provider;
        }

        public async Task<bool> editProviderNotification(int providerId,bool isNotification)
        {
            PhysicianNotification physicianNotification = _userRepository.GetPhysicianNotification(providerId);
            physicianNotification.IsNotificationStopped[0] = isNotification;
            return await _userRepository.updatePhysicianNotification(physicianNotification);
        }

        public bool contactProvider(ContactProvider model)
        {
            if(model.email)
            {
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                    Subject = "Message From Admin",
                    IsBodyHtml = true,
                    Body = model.Message,
                };
                //mailMessage.To.Add(model.Email);
                mailMessage.To.Add("tatva.dotnet.avinashpatel@outlook.com");
                SmtpClient smtpClient = new SmtpClient("smtp.office365.com")
                {
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Port = 587,
                    Credentials = new NetworkCredential(userName: "tatva.dotnet.avinashpatel@outlook.com", password: "Avinash@6351"),
                };
                try
                {
                    smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public CreateProvider GetCreateProvider()
        {
            Dictionary<int, string> regions = new Dictionary<int, string>();
            List<Region> allRegion = _requestClientRepository.getAllRegions();
            foreach (Region region in allRegion)
            {
                regions.Add(region.RegionId, region.Name);
            } 
            Dictionary<int, string> roles = new Dictionary<int, string>();
            List<Role> allRoles = _roleRepository.getRolesByUserType(3);
            foreach (Role role in allRoles)
            {
                roles.Add(role.RoleId, role.Name);
            }
            CreateProvider createProvider = new CreateProvider()
            {
                Regions = regions,
                Roles = roles,
            };
            return createProvider;
        }

        public async Task<bool> createProvider(CreateProvider model)
        {
            int aspNetRoleId = _aspRepository.checkUserRole(role: "Physician");
            if (aspNetRoleId == 0)
            {
                AspNetRole aspNetRole = new()
                {
                    Name = "Physician",
                };
                aspNetRoleId = await _aspRepository.addUserRole(aspNetRole);
            }
            AspNetUser aspNetUser = new()
            {
                UserName = model.FirstName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                PasswordHash = genrateHash(model.Password),
                CreatedDate = DateTime.Now,
            };
            int aspNetUserId = await _aspRepository.addUser(aspNetUser);
            AspNetUserRole aspNetUserRole = new()
            {
                UserId = aspNetUserId,
                RoleId = aspNetRoleId,
            };
            await _aspRepository.addAspNetUserRole(aspNetUserRole);
            filePickUp("Photo", aspNetUserId, model.Photo);
            if (model.IsAgreementDoc)
            {
                filePickUp("AgreementDoc", aspNetUserId, model.AgreementDoc);
            }
            if (model.IsBackgroundDoc)
            {
                filePickUp("BackgroundDoc", aspNetUserId, model.BackgroundDoc);
            }
            if (model.IsHIPAACompliance)
            {
                filePickUp("HIPAACompliance", aspNetUserId, model.HIPAACompliance);
            }
            if (model.IsNonDisclosureDoc)
            {
                filePickUp("NonDisclosureDoc", aspNetUserId, model.NonDisclosureDoc);
            }
            Physician physician = new Physician()
            {
                AspNetUserId = aspNetUserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Mobile = model.Phone,
                MedicalLicense = model.MedicalLicance,
                Address1 = model.Add1,
                Address2 = model.Add2,
                City = model.City,
                Zip = model.Zip,
                RegionId = int.Parse(model.SelectedRegion),
                AltPhone = model.Phone2,
                CreatedDate = DateTime.Now,
                Status = 0,
                BusinessName = model.BusinessName,
                BusinessWebsite = model.BusinessWebsite,
                Npinumber = model.NpiNumber,
                IsAgreementDoc = new BitArray(1, model.IsAgreementDoc),
                IsBackgroundDoc = new BitArray(1, model.IsBackgroundDoc),
                IsNonDisclosureDoc = new BitArray(1, model.IsNonDisclosureDoc),
                IsTrainingDoc = new BitArray(1, model.IsHIPAACompliance),
                Photo = model.Photo.FileName
            };
            if (await _userRepository.addPhysician(physician))
            {
                PhysicianNotification physicianNotification = new PhysicianNotification()
                {
                    PhysicianId = physician.PhysicianId,
                    IsNotificationStopped = new BitArray(1, false)
                };
                if(await _userRepository.addPhysicianNotification(physicianNotification))
                {
                    foreach (String regionId in model.SelectedRegions)
                    {
                        PhysicianRegion physicianRegion = new PhysicianRegion()
                        {
                            PhysicianId = physician.PhysicianId,
                            RegionId = int.Parse(regionId),
                        };
                        await _userRepository.addPhysicianRegion(physicianRegion);
                    }
                    physician.IsNotification = physicianNotification.Id;
                    return await _userRepository.updatePhysician(physician);
                }
            }
            return false;
        }

        private String genrateHash(String password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashPassword).Replace("-", "").ToLower();
            }
        }

        private void filePickUp(String folderName,int aspNetUserId,IFormFile file)
        {
            String path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/Providers/"+ folderName +"/" + aspNetUserId.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            };
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = fileInfo.Name;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
    }
}
