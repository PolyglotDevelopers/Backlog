using Backlog.Core.Common;
using Backlog.Core.Domain.WorkItems;
using Backlog.Data.Repository;
using Backlog.Service.Masters;
using Microsoft.AspNetCore.Http;
using System.Linq.Dynamic.Core;

namespace Backlog.Service.WorkItems
{
    public class BacklogItemService : IBacklogItemService
    {
        #region Fields

        protected readonly IRepository<BacklogItem> _backlogItemRepository;
        protected readonly IRepository<BacklogItemDocument> _backlogItemDocRepository;
        protected readonly IDocumentService _documentService;
        protected readonly IWorkContext _workContext;

        #endregion

        #region Ctor
        public BacklogItemService(IRepository<BacklogItem> backlogItemRepository,
            IRepository<BacklogItemDocument> backlogItemDocRepository,
            IDocumentService documentService,
            IWorkContext workContext)
        {
            _backlogItemRepository = backlogItemRepository;
            _backlogItemDocRepository = backlogItemDocRepository;
            _documentService = documentService;
            _workContext = workContext;
        }
        #endregion

        #region Methods

        public async Task<IPagedList<BacklogItem>> GetPagedListAsync(int projectId, string search = "", int pageIndex = 0,
        int pageSize = int.MaxValue, int sortColumn = -1, string sortDirection = "")
        {
            var accessibleProjectIds = new List<int>();

            if (projectId > 0)
            {
                accessibleProjectIds.Add(projectId);
            }
            else
            {
                var accessibleProjects = await _workContext.GetCurrentEmployeeProjectsAsync();
                accessibleProjectIds = accessibleProjects.Select(x => x.Id).ToList();
            }

            return await _backlogItemRepository.GetAllPagedAsync(query =>
            {
                query = query.Where(x => accessibleProjectIds.Contains(x.ProjectId));

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

        public async Task InsertAsync(BacklogItem entity, List<IFormFile> files)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Code = Guid.NewGuid();

            if (entity.ParentId == 0)
                entity.ParentId = null;
            if (entity.ModuleId == 0)
                entity.ModuleId = null;
            if (entity.SubModuleId == 0)
                entity.SubModuleId = null;
            if (entity.SprintId == 0)
                entity.SprintId = null;
            if (entity.AssigneeId == 0)
                entity.AssigneeId = null;

            var newEntity = await _backlogItemRepository.InsertAndGetAsync(entity);
            if (newEntity != null && newEntity.Id > 0 && files != null)
            {
                foreach (var file in files)
                {
                    var doc = await _documentService.InsertAndGetAsync(file);
                    if (doc != null && doc.Id > 0)
                    {
                        await _backlogItemDocRepository.InsertAsync(new BacklogItemDocument
                        {
                            DocumentId = doc.Id,
                            BacklogItemId = newEntity.Id,
                            CreatedOn = newEntity.CreatedOn,
                            CreatedById = newEntity.CreatedById
                        });
                    }
                }
            }
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
