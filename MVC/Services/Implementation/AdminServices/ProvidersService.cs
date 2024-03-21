using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels.Admin;
using System.Net.Mail;
using System.Net;

namespace Services.Implementation.AdminServices
{
    public class ProvidersService : IProvidersService
    {
        private readonly IUserRepository _userRepository;
        private IRequestClientRepository _requestClientRepository;

        public ProvidersService(IUserRepository userRepository, IRequestClientRepository requestClientRepository)
        {
            _userRepository = userRepository;
            _requestClientRepository = requestClientRepository;
        }

        public Provider getProviders(int regionId)
        {
            Dictionary<int, string> regions = new Dictionary<int, string>();
            List<Physician> physicians = new List<Physician> { };
            if(regionId == 0)
            {
                physicians = _userRepository.getAllPhysicians();
                List<Region> allRegion = _requestClientRepository.getAllRegions();
                foreach (Region region in allRegion)
                {
                    regions.Add(region.RegionId, region.Name);
                }
            }
            else
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
    }
}
