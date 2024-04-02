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
        private readonly IShiftRepository _shiftRepository;

        public ProvidersService(IUserRepository userRepository, IRequestClientRepository requestClientRepository, IRoleRepository roleRepository,
                                 IAspRepository aspRepository, IShiftRepository shiftRepository)
        {
            _userRepository = userRepository;
            _requestClientRepository = requestClientRepository;
            _roleRepository = roleRepository;
            _aspRepository = aspRepository;
            _shiftRepository = shiftRepository;
        }

        public Provider getProviders(int regionId)
        {
            Dictionary<int, string> regions = new Dictionary<int, string>();
            if(regionId == 0)  // for first time page load - on filter this part not execute
            {
                regions = _requestClientRepository.getAllRegions().ToDictionary(region => region.RegionId, region => region.Name);
            }
            List<ProviderTable> providerTables = _userRepository.getAllPhysiciansByRegionId(regionId).Select(
            physician => new ProviderTable()
            {
                FirstName = physician.FirstName,
                LastName = physician.LastName,
                Notification = physician.PhysicianNotifications.FirstOrDefault().IsNotificationStopped[0],
                providerId = physician.PhysicianId,
                Status = physician.Status == 1 ? "Active" : "Pending",
            }).ToList();
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
                //Physician physician = _userRepository.getPhysicianByPhysicianId(model.providerId);
                //mailMessage.To.Add(physician.Email);
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

        public CreateProvider getCreateProvider()
        {
            CreateProvider createProvider = new CreateProvider()
            {
                Regions = _requestClientRepository.getAllRegions().ToDictionary(region => region.RegionId, region => region.Name),
                Roles = _roleRepository.getRolesByUserType(3).ToDictionary(role => role.RoleId, role => role.Name),
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
                Photo = model.Photo.FileName,
                AdminNotes = model.AdminNotes,
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

        public ProviderScheduling getProviderSchedulingData()
        {
            List<SchedulingTable> schedulingTables = _userRepository.getAllPhysiciansByRegionId(0).Select(
                                                                   physician => _dayWiseScheduling(physician, DateTime.Now)).ToList();
            CreateShift createShift = new CreateShift()
            {
                Regions = _requestClientRepository.getAllRegions().ToDictionary(region => region.RegionId, region => region.Name),
            };
            ProviderScheduling providerScheduling = new ProviderScheduling()
            {
                TableData = schedulingTables,
                CreateShift = createShift,
            };
            return providerScheduling;
        }

        public async Task<bool> createShift(CreateShift model,int aspNetUserId)
        {
            Shift shift = new Shift()
            {
                PhysicianId = model.SelectedPhysician,
                StartDate = new DateOnly(model.ShiftDate.Year,model.ShiftDate.Month,model.ShiftDate.Day),
                IsRepeat = new BitArray(1, model.IsRepeat),
                WeekDays = model.SelectedDays.ToString(),
                RepeatUpto = model.RepeatEnd,
                CreatedBy = aspNetUserId,
                CreatedDate = DateTime.UtcNow,
            };
            if(await _shiftRepository.addShift(shift))
            {
                if(model.IsRepeat)
                {
                    model.SelectedDays.ForEach(async day =>
                    {
                        for(int i=0;i<model.RepeatEnd;i++)
                        {
                            ShiftDetail shiftDetail = new ShiftDetail()
                            {
                                ShiftId = shift.ShiftId,
                                ShiftDate = model.ShiftDate.AddDays(day - (int)model.ShiftDate.DayOfWeek),
                                RegionId = model.SelectedRegion,
                                StartTime = model.StartTime,
                                EndTime = model.EndTime,
                                Status = 0,
                                IsDeleted = new BitArray(1, false)
                            };
                            await _shiftRepository.addShiftDetails(shiftDetail);
                        }
                    });
                    return true;
                }
            }
            return false;
        }

        public List<SchedulingTable> getSchedulingTableDate(int regionId, int type, string time)
        {
            switch (type)
            {
                default:
                case 3:
                case 1: return _userRepository.getAllPhysiciansByRegionId(regionId).Select(
                                                        physician => _dayWiseScheduling(physician, DateTime.Parse(time))).ToList();
                case 2: return _userRepository.getAllPhysiciansByRegionId(regionId).Select(
                                                        physician => _weekWiseScheduling(physician, DateTime.Parse(time))).ToList();
            }
        }

        private SchedulingTable _dayWiseScheduling(Physician physician,DateTime time)
        {
            string path = "/Files//Providers/Photo/";
            List<ShiftDetailsDayWise> dayWise = new List<ShiftDetailsDayWise>();
            List<ShiftDetail> shiftDetails = _shiftRepository.getShiftDetailByPhysicianId(physician.PhysicianId,time,time);
            foreach (ShiftDetail shiftDetail in shiftDetails)
            {
                int totalHalfHour = (int)(shiftDetail.EndTime - shiftDetail.StartTime).TotalMinutes / 30;
                dayWise.AddRange(
                    Enumerable.Range(shiftDetail.StartTime.Hour, totalHalfHour % 2 == 0 ? totalHalfHour / 2 : (totalHalfHour / 2) + 1)
                    .Select(time => new ShiftDetailsDayWise()
                    {
                        Status = shiftDetail.Status == 0 ? "bg-pink" : "bg-success",
                        Time = time,
                    }).ToList()
                );
                if (totalHalfHour % 2 == 0)
                {
                    if (shiftDetail.StartTime.Minute == 30)
                    {
                        dayWise.FirstOrDefault(shiftDetailsDayWise => shiftDetailsDayWise.Time == shiftDetail.StartTime.Hour).SecoundHalf = true;
                        dayWise.Add(new ShiftDetailsDayWise()
                        {
                            Status = shiftDetail.Status == 0 ? "bg-pink" : "bg-success",
                            Time = shiftDetail.EndTime.Hour,
                            FirstHalf = true,
                        });
                    }
                }
                else
                {
                    if (shiftDetail.StartTime.Minute == 30)
                    {
                        dayWise.FirstOrDefault(shiftDetailsDayWise => shiftDetailsDayWise.Time == shiftDetail.StartTime.Hour).SecoundHalf = true;
                    }
                    else
                    {
                        dayWise.FirstOrDefault(shiftDetailsDayWise => shiftDetailsDayWise.Time == shiftDetail.EndTime.Hour).FirstHalf = true;
                    }
                }
            }
            return new SchedulingTable()
            {
                Photo = path + physician.AspNetUserId + "/" + physician.Photo,
                FirstName = physician.FirstName,
                LastName = physician.LastName,
                DayWise = dayWise,
            };
        }

        private SchedulingTable _weekWiseScheduling(Physician physician, DateTime time)
        {
            string path = "/Files//Providers/Photo/";
            Dictionary<int, double> weekWise = new Dictionary<int, double>();
            List <ShiftDetail> shiftDetails = _shiftRepository.getShiftDetailByPhysicianId(physician.PhysicianId, time, time.AddDays(6));
            foreach(ShiftDetail shiftDetail in shiftDetails)
            {
                double shiftHours = (shiftDetail.EndTime - shiftDetail.StartTime).TotalHours;
                if (weekWise.ContainsKey((int)shiftDetail.ShiftDate.DayOfWeek))
                {
                    weekWise[(int)shiftDetail.ShiftDate.DayOfWeek] +=  shiftHours;
                }
                else
                {
                    weekWise.Add((int)shiftDetail.ShiftDate.DayOfWeek, shiftHours);
                }
            }
            return new SchedulingTable()
            {
                Photo = path + physician.AspNetUserId + "/" + physician.Photo,
                FirstName = physician.FirstName,
                LastName = physician.LastName,
                WeekWise = weekWise,
            };
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
