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

        #region Members Mapping

        Task<IPagedList<ProjectMemberMap>> GetPagedListMembersAsync(int projectId, string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "");

        Task<ProjectMemberMap> GetMemberByIdAsync(int id);

        Task<ProjectMemberMap> GetMemberByIdAndProjectAsync(int employeeId, int projectId);

        Task InsertMemberAsync(ProjectMemberMap entity);

        Task UpdateMemberAsync(ProjectMemberMap entity);

        Task DeleteMemberAsync(ProjectMemberMap entity);

        #endregion
    }
}
