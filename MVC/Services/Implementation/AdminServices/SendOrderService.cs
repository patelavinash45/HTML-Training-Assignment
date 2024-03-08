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
            List<HealthProfessional> healthProfessionals = _healthProfessionalRepository.getHealthProfessional();
            Dictionary<int, Tuple<int,String>> businesses = new Dictionary<int, Tuple<int, String>>();
            foreach (var item in healthProfessionals)
            {
                businesses.Add(item.VendorId, new Tuple<int, string>(item.Profession, item.VendorName));
            }
            SendOrder sendOrder = new SendOrder()
            {
                RequestId = requestId,
                Professions = professions,
                Businesses = businesses,
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
    }
}
