using Repositories.DataModels;

namespace Repositories.Interfaces
{
    public interface IRoleRepository
    {
        List<Role> getAllRoles();

        List<Role> getRolesByUserType(int type);

        Role getRoleByRoleId(int roleId);

        List<Menu> getAllMenus();

        List<Menu> getAllMenusByRole(int roleId);

        List<RoleMenu> getAllRoleMenusByRole(int roleId);

        Task<int> addRole(Role role);

        Task<bool> addRoleMenu(RoleMenu roleMenu);

        Task<bool> updateRole(Role role);

        Task<bool> deleteRoleMenu(RoleMenu roleMenu);
    }
}
