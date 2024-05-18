using Backlog.Core.Common;
using Backlog.Core.Domain.WorkItems;
using Backlog.Data.Repository;
using System.Linq.Dynamic.Core;

namespace Backlog.Service.WorkItems
{
    public class BacklogItemService : IBacklogItemService
    {
        #region Fields

        protected readonly IRepository<BacklogItem> _backlogItemRepository;

        #endregion

        #region Ctor
        public BacklogItemService(IRepository<BacklogItem> backlogItemRepository)
        {
            _backlogItemRepository = backlogItemRepository;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<BacklogItem>> GetPagedListAsync(int projectId, string search = "", int pageIndex = 0,
        int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            return await _backlogItemRepository.GetAllPagedAsync(query =>
            {
                if (projectId > 0)
                    query = query.Where(x => x.ProjectId == projectId);

                if (sortColumn >= 0)
                {
                    var propertyInfo = typeof(BacklogItem).GetProperties();
                    var curOrderBy = propertyInfo[sortColumn].Name + " " + sortDirection;
                    query = query.OrderBy(curOrderBy);
                }
                else
                {
                    query = query.OrderBy(x => x.CreatedBy);
                }

                if (!string.IsNullOrWhiteSpace(search))
                    query = query.Where(c => c.Title.Contains(search) ||
                    c.Description.Contains(search));

                return query;
            }, pageIndex, pageSize);
        }
        public async Task<IList<BacklogItem>> GetAllAsync(int projectId)
        {
            return await _backlogItemRepository.GetAllAsync(query =>
            {
                if (projectId > 0)
                    query = query.Where(x => x.ProjectId == projectId);

                return query;
            }, false);
        }

        public async Task<BacklogItem> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _backlogItemRepository.GetByIdAsync(id);
        }

        public async Task InsertAsync(BacklogItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _backlogItemRepository.InsertAsync(entity);
        }

        public async Task UpdateAsync(BacklogItem entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _backlogItemRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(BacklogItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _backlogItemRepository.DeleteAsync(entity);
        }

        #endregion
    }
}
