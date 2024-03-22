using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IAccessService
    {
        Access getAccessData();

        CreateRole getCreateRole();

        Dictionary<int,String> getMenusByRole(int roleId);

        Task<bool> createRole(string data);

        Task<bool> delete(int roleId);
    }
}
