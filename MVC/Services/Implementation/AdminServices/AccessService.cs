using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels.Admin;
using System.Collections;
using System.Text.Json;

namespace Services.Implementation.AdminServices
{
    public class AccessService : IAccessService
    {
        private readonly IRoleRepository _roleRepository;

        public AccessService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Access getAccessData()
        {
            List<Role> roles = _roleRepository.getAllRoles();
            List<AccessTable> accessTables = new List<AccessTable>();
            foreach (Role role in roles)
            {
                AccessTable accessTable = new AccessTable()
                {
                    Name = role.Name,
                    AccountType = role.AccountTypeNavigation.Name,
                    RoleId = role.RoleId,
                };
                accessTables.Add(accessTable);
            }
            Access access = new Access()
            {
                RolesData = accessTables,
            };
            return access;
        }

        public CreateRole getCreateRole()
        {
            List<Menu> allMenus = _roleRepository.getAllMenus();
            Dictionary<int,String> menus = new Dictionary<int,String>();
            foreach (Menu menu in allMenus)
            {
                menus.Add(menu.MenuId, menu.Name);
            }
            CreateRole createRole = new CreateRole()
            {
                Menus = menus,
            };
            return createRole;
        }

        public Dictionary<int, String> getMenusByRole(int roleId)
        {
            List<Menu> allMenus = _roleRepository.getAllMenusByRole(roleId);
            Dictionary<int, String> menus = new Dictionary<int, String>();
            foreach (Menu menu in allMenus)
            {
                menus.Add(menu.MenuId, menu.Name);
            }
            return menus;
        }

        public async Task<bool> createRole(string data)
        {
            CreateRole createRole = JsonSerializer.Deserialize<CreateRole>(data);
            Role role = new Role()
            {
                Name = createRole.RoleName,
                AccountType = int.Parse(createRole.SlectedAccountType),
                CreatedDate = DateTime.Now,
            };
            int roleId = await _roleRepository.addRole(role); 
            if(roleId > 0)
            {
                foreach(String menuId in createRole.SelectedMenus)
                {
                    RoleMenu roleMenu = new RoleMenu()
                    {
                        RoleId = roleId,
                        MenuId = int.Parse(menuId),
                    };
                    await _roleRepository.addRoleMenu(roleMenu);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> delete(int roleId)
        {
            List<RoleMenu> roleMenus = _roleRepository.getAllRoleMenusByRole(roleId);
            foreach(RoleMenu roleMenu in roleMenus)
            {
                await _roleRepository.deleteRoleMenu(roleMenu);
            }
            Role role = _roleRepository.getRoleByRoleId(roleId);
            role.IsDeleted = new BitArray(1, true);
            return await _roleRepository.updateRole(role);
        }
    }
}
