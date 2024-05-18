using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface ICompanyService
    {
        Task<Company> GetByIdAsync(int id);

        Task UpdateAsync(Company entity);
    }
}
