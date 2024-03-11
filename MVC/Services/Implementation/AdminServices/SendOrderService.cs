using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels.Admin;

namespace Services.Implementation.AdminServices
{
    public class SendOrderService : ISendOrderService
    {
        private readonly IHealthProfessionalRepository _healthProfessionalRepository;

        public SendOrderService(IHealthProfessionalRepository healthProfessionalRepository)
        {
            _healthProfessionalRepository = healthProfessionalRepository;
        }

        public SendOrder getSendOrderDetails(int requestId)
        { 
            List<HealthProfessionalType> healthProfessionalTypes = _healthProfessionalRepository.getHealthProfessionalTypes();
            Dictionary<int,String> professions = new Dictionary<int,String>();
            foreach(var item in healthProfessionalTypes)
            {
                professions.Add(item.HealthProfessionalId, item.ProfessionName);
            }
            SendOrder sendOrder = new SendOrder()
            {
                RequestId = requestId,
                Professions = professions,
            };
            return sendOrder;
        }

        public HealthProfessional getBussinessData(int venderId)
        {
            return _healthProfessionalRepository.getHealthProfessional(venderId);
        }

        public async Task<int> addOrderDetails(SendOrder model)
        {
            OrderDetail orderDetail = new OrderDetail()
            {
                VendorId = model.SelectedBusiness,
                RequestId = model.RequestId,
                FaxNumber = model.FaxNumber,
                Email = model.Email,
                BusinessContact = model.Contact,
                Prescription = model.OrderDetails,
                NoOfRefill = model.NoOfRefill,
                CreatedDate = DateTime.Now,
            };
            return await _healthProfessionalRepository.addOrderDetails(orderDetail);
        }

        public Dictionary<int, string> getBussinessByProfession(int professionId)
        {
            List<HealthProfessional> healthProfessionals = _healthProfessionalRepository.getHealthProfessionalByProfession(professionId);
            Dictionary<int, String> businesses = new Dictionary<int, String>();
            foreach (var item in healthProfessionals)
            {
                businesses.Add(item.VendorId, item.VendorName);
            }
            return businesses;
        }
    }
}
