using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface ISettingService
    {
        Task<IPagedList<Setting>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "");

        Task<Setting> GetByIdAsync(int id);

        Task<Setting> GetByNameAsync(string name);

        Task<string> GetStringValueByNameAsync(string name);

        Task<decimal> GetDecimalValueByNameAsync(string name);

        Task<bool> GetBoolValueByNameAsync(string name);

        Task<int> GetIntValueByNameAsync(string name);

        Task<List<string>> GetAllowedIpAddressAsync();

        Task<Setting> SaveAsync(Setting setting);

        Task SaveAsync(string name, object value);
    }
}