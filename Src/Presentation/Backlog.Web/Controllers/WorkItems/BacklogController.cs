using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.WorkItems;
using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Service.WorkItems;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Masters;
using Backlog.Web.Models.WorkItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Controllers.WorkItems
{
    public class BacklogController : BaseController
    {
        #region Fields

        protected readonly IBacklogItemService _backlogItemService;
        protected readonly ITaskTypeService _taskTypeService;
        protected readonly ISeverityService _severityService;
        protected readonly IProjectService _projectService;
        protected readonly IModuleService _moduleService;
        protected readonly ISubModuleService _subModuleService;
        //protected readonly ISprintService _backlogItemService;
        protected readonly IEmployeeService _employeeService;
        protected readonly IStatusService _statusService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public BacklogController(IBacklogItemService backlogItemService,
            ITaskTypeService taskTypeService,
            ISeverityService severityService,
            IProjectService projectService,
            IModuleService moduleService,
            ISubModuleService subModuleService,
            IEmployeeService employeeService,
            IStatusService statusService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _backlogItemService = backlogItemService;
            _taskTypeService = taskTypeService;
            _severityService = severityService;
            _projectService = projectService;
            _moduleService = moduleService;
            _subModuleService = subModuleService;
            _employeeService = employeeService;
            _statusService = statusService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _workContext = workContext;
            _mapper = mapper;

        }

        #endregion

        #region Actions

        public async Task<IActionResult> Index()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDenied();

            var model = new BacklogItemModel();
            await InitModelAsync(model);

            return View(model);
        }

        [HttpPost, FormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(BacklogItemModel model, bool continueEditing)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var loggedEmployee = await _workContext.GetCurrentEmployeeAsync();
                var entity = _mapper.Map<BacklogItem>(model);
                entity.CreatedOn = DateTime.Now;
                entity.CreatedById = loggedEmployee.Id;

                await _backlogItemService.InsertAsync(entity, model.Files);

                await _employeeActivityService.InsertAsync("Backlog", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Title), entity);
                return continueEditing ? RedirectToAction("Edit", new { id = entity.Id }) : RedirectToAction("Index");
            }

            await InitModelAsync(model);
            return View(model);
        }

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(int projectId, DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageBacklog))
                return AccessDeniedDataRead();

            var sortColumn = -1;
            var sortDirection = "asc";
            var search = string.Empty;

            if (!string.IsNullOrEmpty(Request.Form["order[0][column]"]))
            {
                sortColumn = int.Parse(Request.Form["order[0][column]"]);
            }

            if (!string.IsNullOrEmpty(Request.Form["order[0][dir]"]))
            {
                sortDirection = Request.Form["order[0][dir]"];
            }

            if (!string.IsNullOrEmpty(Request.Form["search[value]"]))
            {
                search = Request.Form["search[value]"];
            }

            var data = await _backlogItemService.GetPagedListAsync(projectId, search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => new BacklogItemGridModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    DueDate = x.DueDate,
                    Project = x.Project.Name,
                    Module = x.Module?.Name,
                    SubModule = x.SubModule?.Name,
                    Sprint = x.Sprint?.Name,
                    Assignee = x.Assignee?.Name,
                    ReOpenCount = x.ReOpenCount,
                    SubTaskCount = x.SubTaskCount,
                    TaskType = _mapper.Map<TaskTypeListModel>(x.TaskType),
                    Severity = _mapper.Map<SeverityListModel>(x.Severity),
                    Status = _mapper.Map<StatusListModel>(x.Status)
                }),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion

        #region Helper

        private async Task InitModelAsync(BacklogItemModel model)
        {
            var taskTypes = await _taskTypeService.GetAllActiveAsync();
            var severities = await _severityService.GetAllActiveAsync();
            var projects = await _projectService.GetAllActiveAsync();
            var modules = await _moduleService.GetAllActiveAsync();
            //var subModules = await _projectService.GetAllActiveAsync();
            //var sprints = await _sprintService.GetAllActiveAsync();
            //var assinees = await _employeeService.GetAllActiveAsync();
            var status = await _statusService.GetAllActiveAsync();

            foreach (var item in taskTypes)
            {
                model.AvailableTaskTypes.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.TaskTypeId
                });
            }

            foreach (var item in severities)
            {
                model.AvailableSeverities.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.SeverityId
                });
            }

            foreach (var item in projects)
            {
                model.AvailableProjects.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.ProjectId
                });
            }

            foreach (var item in modules)
            {
                model.AvailableModules.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.ModuleId
                });
            }

            foreach (var item in status)
            {
                model.AvailableStatus.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.StatusId
                });
            }
        }

        #endregion

        #region Ajax

        public async Task<List<SelectListItem>> GetTaskType()
        {
            var states = await _taskTypeService.GetAllActiveAsync(cacheData: true);
            return states.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetSeverity()
        {
            var states = await _severityService.GetAllActiveAsync(cacheData: true);
            return states.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetStatus()
        {
            var states = await _statusService.GetAllActiveAsync(cacheData: true);
            return states.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetAssignedTo(int? projectId)
        {
            var states = await _projectService.GetAllActiveAsync(cacheData: true);
            return states.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
        }

        #endregion
    }
}