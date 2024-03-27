using Microsoft.EntityFrameworkCore;
using Repositories.DataContext;
using Repositories.DataModels;
using Repositories.Interfaces;
using System.Collections;
using System.Data;

namespace Repositories.Implementation
{
    public class RoleRepository : IRoleRepository
    {
        private readonly HalloDocDbContext _dbContext;

        public RoleRepository(HalloDocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Role> getAllRoles()
        {
            return _dbContext.Roles.Include(a => a.AccountTypeNavigation).Where(a => a.IsDeleted != new BitArray(1, true)).ToList();
        }

        public List<Role> getRolesByUserType(int type)
        {
            return _dbContext.Roles.Include(a => a.AccountTypeNavigation).Where(a => a.IsDeleted != new BitArray(1, true) && a.AccountType == type).ToList();
        }

        public Role getRoleByRoleId(int roleId)
        {
            return _dbContext.Roles.FirstOrDefault(a => a.RoleId == roleId);
        }

        public List<Menu> getAllMenus()
        {
            return _dbContext.Menus.ToList();
        }

        public List<Menu> getAllMenusByRole(int roleId)
        {
            return _dbContext.Menus.Where(a => a.AccountType == roleId).ToList();
        }

        public List<RoleMenu> getAllRoleMenusByRole(int roleId)
        {
            return _dbContext.RoleMenus.Where(a => a.RoleId == roleId).ToList();
        }

        public async Task<int> addRole(Role role)
        {
            _dbContext.Roles.Add(role);
            await _dbContext.SaveChangesAsync();
            return role?.RoleId ?? 0;
        }

        public async Task<bool> addRoleMenu(RoleMenu roleMenu)
        {
            _dbContext.RoleMenus.Add(roleMenu);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> updateRole(Role role)
        {
            _dbContext.Roles.Update(role);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> deleteRoleMenu(RoleMenu roleMenu)
        {
            _dbContext.RoleMenus.Remove(roleMenu);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
