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
using System.Text.Json;

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

        public List<ProviderLocation> getProviderLocation()
        {
            return _userRepository.getAllProviderLocation().Select(physicianLocation => new ProviderLocation
            {
                ProviderName = physicianLocation.PhysicianName,
                latitude = physicianLocation.Latitude,
                longitude = physicianLocation.Longitude,
            }).ToList();
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
                WeekDays = model.IsRepeat ? String.Join("",model.SelectedDays):  "",
                RepeatUpto = model.RepeatEnd,
                CreatedBy = aspNetUserId,
                CreatedDate = DateTime.Now,
            };
            if(await _shiftRepository.addShift(shift))
            {
                ShiftDetail shiftDetail = new ShiftDetail()
                {
                    ShiftId = shift.ShiftId,
                    ShiftDate = model.ShiftDate,
                    RegionId = model.SelectedRegion,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Status = 0,
                    IsDeleted = new BitArray(1, false)
                };
                await _shiftRepository.addShiftDetails(shiftDetail);
                if (model.IsRepeat)
                {
                    foreach(int day in model.SelectedDays)
                    {
                        DateTime date = model.ShiftDate;
                        for(int i=0;i<model.RepeatEnd;i++)
                        {
                            date = date.AddDays(day - (int)date.DayOfWeek + 7);
                            shiftDetail = new ShiftDetail()
                            {
                                ShiftId = shift.ShiftId,
                                ShiftDate = date,
                                RegionId = model.SelectedRegion,
                                StartTime = model.StartTime,
                                EndTime = model.EndTime,
                                Status = 0,
                                IsDeleted = new BitArray(1, false)
                            };
                            await _shiftRepository.addShiftDetails(shiftDetail);
                        }
                    };
                    return true;
                }
            }
            return false;
        }

        public RequestedShift getRequestedShift()
        {
            RequestedShift requestedShift = new RequestedShift()
            {
                Regions = _requestClientRepository.getAllRegions().ToDictionary(region => region.RegionId, region => region.Name),
                RequestedShiftModel = getRequestShiftTableDate(0, false, 1),
            };
            return requestedShift;
        }

        public RequestShiftModel getRequestShiftTableDate(int regionId, bool isMonth, int pageNo)
        {
            int skip = (pageNo - 1) * 10;
            DateTime date = new DateTime();
            if (isMonth)
            {
                date = DateTime.Now;
                date = new DateTime(date.Year, date.Month, 1);
            }
            int totalShifts = _shiftRepository.countAllShiftDetails(regionId, isMonth, date);
            List<RequestedShiftTable> requestedShiftTables = _shiftRepository.getAllShiftDetails(regionId, isMonth, date, skip).Select(
                shiftDetails => new RequestedShiftTable
                {
                    Name = shiftDetails.Shift.Physician.FirstName + " " + shiftDetails.Shift.Physician.LastName,
                    Date = shiftDetails.ShiftDate,
                    StartTime = shiftDetails.StartTime,
                    EndTime = shiftDetails.EndTime,
                    Region = shiftDetails.Region.Name,
                    ShiftDetailsId = shiftDetails.ShiftDetailId,
                }).ToList();
            int totalPages = totalShifts % 10 != 0 ? (totalShifts / 10) + 1 : totalShifts / 10;
            RequestShiftModel requestShiftModel = new RequestShiftModel()
            {
                TotalShifts = totalShifts,
                IsFirstPage = pageNo != 1,
                IsLastPage = pageNo != totalPages,
                IsNextPage = pageNo < totalPages,
                IsPreviousPage = pageNo > 1,
                PageNo = pageNo,
                StartRange = skip + 1,
                EndRange = skip + 10 < totalShifts ? skip + 10 : totalShifts,
                RequestedShiftTables = requestedShiftTables,
            };
            return requestShiftModel;
        }

        public async Task<bool> chnageShiftDetails(string dataList,bool isApprove)
        {
            List<int> ids = JsonSerializer.Deserialize<List<String>>(dataList).Select(id => int.Parse(id)).ToList();
            if (isApprove)
            {
                foreach (int id in ids)
                {
                    ShiftDetail shiftDetail = _shiftRepository.getShiftDetails(id);
                    shiftDetail.Status = 1;
                    await _shiftRepository.updateShiftDetails(shiftDetail);
                }
                return true;
            }
            else
            {
                foreach (int id in ids)
                { 
                    ShiftDetail shiftDetail = _shiftRepository.getShiftDetails(id);
                    shiftDetail.IsDeleted = new BitArray(1, true);
                    await _shiftRepository.updateShiftDetails(shiftDetail);
                };
                return true;
            }
        }

        public SchedulingTableMonthWise monthWiseScheduling(string dateString)
        {
            DateTime date = DateTime.Parse(dateString);
            int startDate = (int)date.DayOfWeek;
            Dictionary<int, List<ShiftDetailsMonthWise>> monthWiseScheduling = new Dictionary<int, List<ShiftDetailsMonthWise>>();
            List<ShiftDetail> shiftDetails = _shiftRepository.getAllShiftDetailsForSecificMonths(startDate: date, endDate: date.AddMonths(1).AddDays(-1));
            int totalDays = DateTime.DaysInMonth(date.Year, date.Month);
            for(int i = 0;i < totalDays;i++)
            {
                List<ShiftDetailsMonthWise> shiftDetailsMonthWises = new List<ShiftDetailsMonthWise>();
                shiftDetails.Where(a => a.ShiftDate == date.AddDays(i)).ToList().ForEach(shiftDetail =>
                {
                    double shiftHours = (shiftDetail.EndTime - shiftDetail.StartTime).TotalHours;
                    ShiftDetailsMonthWise shiftDetailsMonthWise = shiftDetailsMonthWises.FirstOrDefault(a => a.PhysicianId == shiftDetail.Shift.PhysicianId);
                    if(shiftDetailsMonthWise != null)
                    {
                        shiftDetailsMonthWise.Time += shiftHours;
                    }
                    else
                    {
                        shiftDetailsMonthWises.Add(new ShiftDetailsMonthWise()
                        {
                            PhysicianId = shiftDetail.Shift.PhysicianId,
                            ProviderName = shiftDetail.Shift.Physician.FirstName + " " + shiftDetail.Shift.Physician.LastName,
                            Time = shiftHours,
                        });
                    }
                });
                if(shiftDetailsMonthWises.Count != 0)
                {
                    monthWiseScheduling.Add(i + 1, shiftDetailsMonthWises);
                }
            }
            SchedulingTableMonthWise schedulingTableMonthWise = new SchedulingTableMonthWise()
            {
                MonthWise = monthWiseScheduling,
                StartDate = startDate,
                TotalDays = totalDays,
            };
            return schedulingTableMonthWise;
        }

        public List<SchedulingTable> getSchedulingTableDate(int regionId, int type, string date)
        {
            switch (type)
            {
                case 1: return _userRepository.getAllPhysiciansByRegionId(regionId).Select(
                                                        physician => _dayWiseScheduling(physician, DateTime.Parse(date))).ToList();
                case 2: return _userRepository.getAllPhysiciansByRegionId(regionId).Select(
                                                        physician => _weekWiseScheduling(physician, DateTime.Parse(date))).ToList();
            }
            return null;
        }

        private SchedulingTable _dayWiseScheduling(Physician physician,DateTime date)
        {
            string path = "/Files//Providers/Photo/";
            List<ShiftDetailsDayWise> dayWise = new List<ShiftDetailsDayWise>();
            List<ShiftDetail> shiftDetails = _shiftRepository.getShiftDetailByPhysicianId(physician.PhysicianId,date,date);
            foreach (ShiftDetail shiftDetail in shiftDetails)
            {
                int totalHalfHour = (int)(shiftDetail.EndTime - shiftDetail.StartTime).TotalMinutes / 30;
                dayWise.AddRange(
                    Enumerable.Range(shiftDetail.StartTime.Hour, totalHalfHour % 2 == 0 ? totalHalfHour / 2 : (totalHalfHour / 2) + 1)
                    .Select(time => new ShiftDetailsDayWise()
                    {
                        Status = shiftDetail.Status == 0 ? "/images/PinkColorImage.jpg" : "/images/SuccessColorImage.jpg",
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
                            Status = shiftDetail.Status == 0 ? "/images/PinkColorImage.jpg" : "/images/SuccessColorImage.jpg",
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

        private SchedulingTable _weekWiseScheduling(Physician physician, DateTime date)
        {
            string path = "/Files//Providers/Photo/";
            Dictionary<int, double> weekWise = new Dictionary<int, double>();
            List <ShiftDetail> shiftDetails = _shiftRepository.getShiftDetailByPhysicianId(physician.PhysicianId, date, date.AddDays(6));
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
