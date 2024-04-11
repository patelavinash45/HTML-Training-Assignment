using Repositories.DataModels;
using Repositories.Interface;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels.Admin;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Services.Implementation.AdminServices
{
    public class AccessService : IAccessService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRequestClientRepository _requestClientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAspRepository _aspRepository;

        public AccessService(IRoleRepository roleRepository, IRequestClientRepository requestClientRepository,IAspRepository aspRepository,
                                         IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _requestClientRepository = requestClientRepository;
            _aspRepository = aspRepository;
            _userRepository = userRepository;
        }

        public Access getAccessData()
        {
            return new Access()
            {
                RolesData = _roleRepository.getAllRoles().Select(role => new AccessTable()
                            {
                                Name = role.Name,
                                AccountType = role.AccountTypeNavigation.Name,
                                RoleId = role.RoleId,
                            }).ToList(),
            };
        }

        public CreateRole getCreateRole()
        {
            return new CreateRole()
            {
                Menus = _roleRepository.getAllMenus().ToDictionary(menu => menu.MenuId, menu => menu.Name),
            };
        }

        public Dictionary<int, String> getMenusByRole(int roleId)
        {
            return _roleRepository.getAllMenusByRole(roleId).ToDictionary(menu => menu.MenuId, menu => menu.Name);
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
                await _roleRepository.addRoleMenus(
                        createRole.SelectedMenus.Select(menuId =>
                        new RoleMenu()
                        {
                            RoleId = roleId,
                            MenuId = int.Parse(menuId),
                        }).ToList()
                );
                return true;
            }
            return false;
        }

        public async Task<bool> delete(int roleId)
        {
            if(await _roleRepository.deleteRoleMenus(_roleRepository.getAllRoleMenusByRole(roleId)))
            {
                Role role = _roleRepository.getRoleByRoleId(roleId);
                role.IsDeleted = new BitArray(1, true);
                return await _roleRepository.updateRole(role);
            }
            return false;
        }

        public AdminCreaateAndProfile GetAdminCreaateAndProfile()
        {
            return new AdminCreaateAndProfile()
            {
                Regions = _requestClientRepository.getAllRegions().ToDictionary(region => region.RegionId, region => region.Name),
                Roles = _roleRepository.getRolesByUserType(2).Select(role => role.Name).ToList(),
            };
        }

        public async Task<bool> createAdmin(AdminCreaateAndProfile model)
        {
            int aspNetRoleId = _aspRepository.checkUserRole(role: "Admin");
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
            Admin admin = new Admin()
            {
                AspNetUserId = aspNetUserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Mobile = model.Mobile,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                RegionId = int.Parse(model.SelectedRegion),
                Zip = model.ZipCode,
                AltPhone = model.Phone,
                Status = model.Status,
                Role = model.SelectedRole,
            };
            if(await _userRepository.addAdmin(admin))
            {
                foreach (String regionId in model.SelectedRegions)
                {
                    AdminRegion adminRegion = new AdminRegion()
                    {
                        AdminId = admin.AdminId,
                        RegionId = int.Parse(regionId),
                    };
                    await _userRepository.addAdminRgion(adminRegion);
                }
                return true;
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
    }
}
