using Backlog.Core.Domain.Employees;

namespace Backlog.Service.Security
{
    public class PermissionProvider : IPermissionProvider
    {
        public static readonly EmployeeRolePermission ManageDashboard = new() { Name = "Manage Dashboard", SystemName = "ManageDashboard", RoleGroup = "Dashboard", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageCompany = new() { Name = "Manage Company", SystemName = "ManageCompany", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageConfiguration = new() { Name = "Manage Configuration", SystemName = "ManageConfiguration", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageEmployee = new() { Name = "Manage Employee", SystemName = "ManageEmployee", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageEmployeeRole = new() { Name = "Manage Employee Role", SystemName = "ManageEmployeeRole", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageDepartment = new() { Name = "Manage Department", SystemName = "ManageDepartment", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageDesignation = new() { Name = "Manage Designation", SystemName = "ManageDesignation", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageCountry = new() { Name = "Manage Country", SystemName = "ManageCountry", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageStateProvince = new() { Name = "Manage State Province", SystemName = "ManageStateProvince", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageCurrency = new() { Name = "Manage Currency", SystemName = "ManageCurrency", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageLanguage = new() { Name = "Manage Language", SystemName = "ManageLanguage", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageLocaleResource = new() { Name = "Manage Locale Resource", SystemName = "ManageLocaleResource", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageEmailAccount = new() { Name = "Manage Email Account", SystemName = "ManageEmailAccount", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageEmailTemplate = new() { Name = "Manage Email Template", SystemName = "ManageEmailTemplate", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageSetting = new() { Name = "Manage Setting", SystemName = "ManageSetting", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageSeverity = new() { Name = "Manage Severity", SystemName = "ManageSeverity", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageStatus = new() { Name = "Manage Status", SystemName = "ManageStatus", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageTaskType = new() { Name = "Manage Task Type", SystemName = "ManageTaskType", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageModule = new() { Name = "Manage Module", SystemName = "ManageModule", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageSubModule = new() { Name = "Manage Sub Module Source", SystemName = "ManageSubModule", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageClient = new() { Name = "Manage Client", SystemName = "ManageClient", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageProject = new() { Name = "Manage Project", SystemName = "ManageProject", RoleGroup = "Configuration", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageSprints = new() { Name = "Manage Sprints", SystemName = "ManageSprints", RoleGroup = "WorkItems", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageBoard = new() { Name = "Manage Boards", SystemName = "ManageBoard", RoleGroup = "WorkItems", SystemPermission = true };
        public static readonly EmployeeRolePermission ManageBacklog = new() { Name = "Manage Backlog", SystemName = "ManageBacklog", RoleGroup = "WorkItems", SystemPermission = true };

        public virtual IEnumerable<EmployeeRolePermission> GetPermissions()
        {
            return
            [
                ManageDashboard,
                ManageConfiguration,
                ManageEmployee,
                ManageEmployeeRole,
                ManageDepartment,
                ManageDesignation,
                ManageCountry,
                ManageStateProvince,
                ManageCurrency,
                ManageLanguage,
                ManageLocaleResource,
                ManageSetting
            ];
        }
    }
}