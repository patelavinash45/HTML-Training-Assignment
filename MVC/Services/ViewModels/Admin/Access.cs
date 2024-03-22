namespace Services.ViewModels.Admin
{
    public class Access
    {
        public List<AccessTable> RolesData;
    }

    public class AccessTable
    {
        public string Name { get; set; }

        public int RoleId { get; set; }

        public string AccountType { get; set; }
    }

    public class CreateRole
    {
        public string RoleName { get; set; }

        public string SlectedAccountType { get; set; }

        public List<string> SelectedMenus { get; set; }

        public Dictionary<int,string>? Menus { get; set; }
    }
}
