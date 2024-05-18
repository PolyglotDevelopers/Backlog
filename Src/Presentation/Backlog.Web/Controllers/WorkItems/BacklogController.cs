using AutoMapper;
using Backlog.Core.Common;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Security;
using Backlog.Service.WorkItems;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.WorkItems;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.WorkItems
{
    public class BacklogController : BaseController
    {
        #region Fields

        protected readonly IBacklogItemService _backlogItemService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public BacklogController(IBacklogItemService backlogItemService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _backlogItemService = backlogItemService;
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
                    Title = x.Title,
                    TaskType = x.TaskType.Name,
                    DueDate = x.DueDate,
                    Project = x.Project.Name,
                    Module = x.Module?.Name,
                    SubModule = x.SubModule?.Name,
                    Sprint = x.Sprint?.Name,
                    Assignee = x.Assignee?.Name,
                    ReOpenCount = x.ReOpenCount,
                    SubTaskCount = x.SubTaskCount,
                    Status = x.Status.Name
                }),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion
    }
}