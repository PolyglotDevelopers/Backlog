using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using System.Linq.Dynamic.Core;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;

namespace Backlog.Service.Masters
{
    public class CurrencyService : ICurrencyService
    {
        #region Fields

        protected readonly IRepository<Currency> _currencyRepository;

        #endregion

        #region Ctor
        public CurrencyService(IRepository<Currency> currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<Currency>> GetPagedListAsync(string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            int sortColumn = -1, string sortDirection = "")
        {
            return await _currencyRepository.GetAllPagedAsync(query =>
            {
                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(Currency).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.Name);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(c =>
                        c.Name.Contains(search) ||
                        c.CurrencySymbol.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }
        public async Task<IList<Currency>> GetAllAsync()
        {
            return await _currencyRepository.GetAllAsync();
        }

        public async Task<IList<Currency>> GetAllActiveAsync()
        {
            return await _currencyRepository.GetAllAsync(q => q.Where(x => x.Active));
        }

        public async Task<Currency> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _currencyRepository.GetByIdAsync(id);
        }

        public async Task<Currency> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var query = from c in _currencyRepository.Table
                        orderby c.Id
                        where c.Name == name
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Currency> GetByCurrencyCodeAsync(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return null;

            var query = from c in _currencyRepository.Table
                        orderby c.Id
                        where c.CurrencyCode == currencyCode
                        select c;
            return await query.FirstOrDefaultAsync();
        }

        public async Task InsertAsync(Currency entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _currencyRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(Currency entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _currencyRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Currency entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _currencyRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
