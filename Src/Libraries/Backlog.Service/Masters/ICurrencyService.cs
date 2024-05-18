using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface ICurrencyService
    {
        Task<IPagedList<Currency>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
           int sortColumn = -1, string sortDirection = "");

        Task<IList<Currency>> GetAllAsync();

        Task<IList<Currency>> GetAllActiveAsync();

        Task<Currency> GetByIdAsync(int id);

        Task<Currency> GetByNameAsync(string name);

        Task<Currency> GetByCurrencyCodeAsync(string currencyCode);

        Task InsertAsync(Currency entity);

        Task UpdateAsync(Currency entity);

        Task DeleteAsync(Currency entity);
    }
}
