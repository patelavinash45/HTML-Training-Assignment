using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IAccessService
    {
        Access getAccessData();

        CreateRole getCreateRole();

        RolesCheckBox getMenusByRole(int roleId);

        Task<bool> createRole(CreateRole model);

        Task<bool> delete(int roleId);

        AdminCreaateAndProfile GetAdminCreaateAndProfile();

        Task<bool> createAdmin(AdminCreaateAndProfile model);

        CreateRole getEditRole(int roleId);
    }
}
