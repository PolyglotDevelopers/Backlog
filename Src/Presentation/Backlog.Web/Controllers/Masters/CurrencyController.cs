using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Domain.Masters;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Datatable;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Controllers.Masters
{
    public class CurrencyController : BaseController
    {
        #region Fields

        protected readonly ICurrencyService _currencyService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public CurrencyController(ICurrencyService currencyService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _currencyService = currencyService;
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
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCurrency))
                return AccessDenied();

            return View();
        }

        public async Task<IActionResult> Create()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCurrency))
                return AccessDeniedPartial();

            var model = new CurrencyModel();

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CurrencyModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCurrency))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = _mapper.Map<Currency>(model);
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                await _currencyService.InsertAsync(entity);

                await _employeeActivityService.InsertAsync("Currency", string.Format(await _localizationService.GetResourceAsync("Log.RecordCreated"), entity.Name), entity);

                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.Success,
                    Message = await _localizationService.GetResourceAsync("Message.SaveSuccess")
                });
            }

            return Json(new JsonResponseModel
            {
                Status = ModelState.IsValid ? HttpStatusCodeEnum.InternalServerError : HttpStatusCodeEnum.ValidationError,
                Message = await _localizationService.GetResourceAsync("Error.Failed"),
                Errors = ModelState.AllErrors()
            });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCurrency))
                return AccessDeniedPartial();

            var entity = await _currencyService.GetByIdAsync(id);
            if (entity == null)
                return NoDataPartial();

            var model = _mapper.Map<CurrencyModel>(entity);

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CurrencyModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCurrency))
                return AccessDeniedPartial();

            if (ModelState.IsValid)
            {
                var entity = await _currencyService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);
                entity.ModifiedOn = DateTime.Now;

                await _currencyService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("Currency", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.Name), entity);

                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.Success,
                    Message = await _localizationService.GetResourceAsync("Message.UpdateSuccess")
                });
            }

            return Json(new JsonResponseModel
            {
                Status = ModelState.IsValid ? HttpStatusCodeEnum.InternalServerError : HttpStatusCodeEnum.ValidationError,
                Message = await _localizationService.GetResourceAsync("Error.Failed"),
                Errors = ModelState.AllErrors()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCurrency))
                return AccessDeniedPartial();

            var entity = await _currencyService.GetByIdAsync(id);
            if (entity == null)
                return Json(new JsonResponseModel
                {
                    Status = HttpStatusCodeEnum.NoData,
                    Message = await _localizationService.GetResourceAsync("FormNoData.Description")
                });

            await _currencyService.DeleteAsync(entity);

            await _employeeActivityService.InsertAsync("Currency", string.Format(await _localizationService.GetResourceAsync("Log.RecordDeleted"), entity.Name), entity);

            return Json(new JsonResponseModel
            {
                Status = HttpStatusCodeEnum.Success,
                Message = await _localizationService.GetResourceAsync("Message.DeleteSuccess")
            });
        }

        #endregion

        #region Data

        [HttpPost]
        public async Task<IActionResult> DataRead(DataSourceRequest request)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCurrency))
                return AccessDeniedDataRead();

            var company = await _workContext.GetCurrentEmployeeAsync();
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

            var data = await _currencyService.GetPagedListAsync(search, request.Start, request.Length,
                sortColumn, sortDirection);

            return Json(new
            {
                request.Draw,
                data = data.Select(x => _mapper.Map<CurrencyModel>(x)),
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            });
        }

        #endregion
    }
}