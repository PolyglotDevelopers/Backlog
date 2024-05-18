using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Backlog.Core.Common;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Masters;

namespace Backlog.Web.Controllers.Masters
{
    public class SettingController : BaseController
    {
        #region Fields

        protected readonly ISettingService _settingService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public SettingController(ISettingService settingService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _settingService = settingService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _workContext = workContext;
            _mapper = mapper;
        }

        #endregion

        #region Action

        public async Task<IActionResult> Index()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSetting))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSetting))
                return AccessDenied();

            var entity = await _settingService.GetByIdAsync(id);
            if (entity == null)
                return RedirectToAction("Index");

            var model = _mapper.Map<SettingModel>(entity);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SettingModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSetting))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = await _settingService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);
                await _settingService.SaveAsync(entity);

                await _employeeActivityService.InsertAsync("Settings", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

                return RedirectToAction("Index");
            }

            return PartialView(model);
        }

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageSetting))
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

            var data = await _settingService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<SettingModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion
    }
}