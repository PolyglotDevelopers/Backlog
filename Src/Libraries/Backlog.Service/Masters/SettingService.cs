using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;

namespace Backlog.Service.Masters
{
    public class SettingService : ISettingService
    {
        #region Fields

        protected readonly IRepository<Setting> _settingsRepository;

        #endregion Fields

        #region Ctors

        public SettingService(IRepository<Setting> settingRepository)
        {
            _settingsRepository = settingRepository;
        }

        #endregion Ctors

        #region Methods

        public async Task<IPagedList<Setting>> GetPagedListAsync(string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _settingsRepository.GetAllPagedAsync(query =>
            {
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Setting).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query =
                        query.Where(
                            c =>
                                c.Name.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }

        public async Task<Setting> GetByIdAsync(int id)
        {
            return await _settingsRepository.GetByIdAsync(id);
        }

        public async Task<Setting> GetByNameAsync(string name)
        {
            var query = from c in _settingsRepository.Table
                        where c.Name == name
                        select c;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<string> GetStringValueByNameAsync(string name)
        {
            var query = from c in _settingsRepository.Table
                        where c.Name == name
                        select c;

            var result = await query.FirstOrDefaultAsync();

            return result?.Value;
        }

        public async Task<decimal> GetDecimalValueByNameAsync(string name)
        {
            var query = from c in _settingsRepository.Table
                        where c.Name == name
                        select c;

            var value = (await query.FirstOrDefaultAsync()).Value;

            return Convert.ToDecimal(value);
        }

        public async Task<bool> GetBoolValueByNameAsync(string name)
        {
            var query = from c in _settingsRepository.Table
                        where c.Name == name
                        select c;

            var value = (await query.FirstOrDefaultAsync()).Value;

            return Convert.ToBoolean(value);
        }

        public async Task<int> GetIntValueByNameAsync(string name)
        {
            var query = from c in _settingsRepository.Table
                        where c.Name == name
                        select c;

            var value = (await query.FirstOrDefaultAsync()).Value;

            return Convert.ToInt32(value);
        }

        public async Task<List<string>> GetAllowedIpAddressAsync()
        {
            var query = from c in _settingsRepository.Table
                        where c.Name == Constant.AllowedIPAddress
                        select c.Value;

            return await query.ToListAsync();
        }

        public async Task<Setting> SaveAsync(Setting setting)
        {
            if (string.IsNullOrEmpty(setting.Value))
                return setting;

            var curSettings = await GetByNameAsync(setting.Name);

            if (curSettings != null)
            {
                curSettings.Value = setting.Value;
                curSettings.ModifiedOn = DateTime.Now;
                await _settingsRepository.UpdateAsync(curSettings);
            }
            else
            {
                setting.CreatedOn = DateTime.Now;
                await _settingsRepository.InsertAsync(setting);
            }

            return curSettings;
        }

        public async Task SaveAsync(string name, object value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value.ToString())) return;

            var curSettings = await GetByNameAsync(name);
            if (curSettings != null)
            {
                curSettings.Value = value.ToString();
                curSettings.ModifiedOn = DateTime.Now;

                await _settingsRepository.UpdateAsync(curSettings);
            }
            else
            {
                await _settingsRepository.InsertAsync(new Setting
                {
                    Name = name,
                    Value = value.ToString(),
                    CreatedOn = DateTime.Now
                });
            }
        }

        #endregion Methods
    }
}