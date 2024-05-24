using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IProjectService
    {
        Task<IPagedList<Project>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Project>> GetAllAsync(bool showDeleted = false);

        Task<IList<Project>> GetAllActiveAsync(bool showDeleted = false, bool cacheData = false);

        Task<Project> GetByIdAsync(int id);

        Task<Project> GetByNameAsync(string name);

        Task InsertAsync(Project entity);

        Task UpdateAsync(Project entity);

        Task DeleteAsync(Project entity);

        #region Employee Mapping

        Task<IPagedList<ProjectEmployeeMap>> GetPagedListEmployeesAsync(int projectId, string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "");

        Task<ProjectEmployeeMap> GetEmployeeByIdAsync(int id);

        Task<ProjectEmployeeMap> GetEmployeeByIdAndProjectAsync(int employeeId, int projectId);

        Task<IList<Project>> GetAllAccessibleProjectsForEmployeeAsync(int employeeId, bool cacheData = false);

        Task InsertEmployeeAsync(ProjectEmployeeMap entity);

        Task UpdateEmployeeAsync(ProjectEmployeeMap entity);

        Task DeleteEmployeeAsync(ProjectEmployeeMap entity);

        #endregion
    }
}
