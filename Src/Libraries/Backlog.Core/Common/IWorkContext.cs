using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Localization;
using Backlog.Core.Domain.Masters;

namespace Backlog.Core.Common
{
    public interface IWorkContext
    {
        Task<Employee> GetCurrentEmployeeAsync();

        Task<Language> GetCurrentEmployeeLanguageAsync();

        Task<List<Project>> GetCurrentEmployeeProjectsAsync();
    }
}
