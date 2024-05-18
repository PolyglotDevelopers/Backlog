using AutoMapper;
using Backlog.Core.Common;
using Backlog.Core.Extensions;
using Backlog.Service.Localization;
using Backlog.Service.Logging;
using Backlog.Service.Masters;
using Backlog.Service.Security;
using Backlog.Web.Controllers.Common;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Controllers.Masters
{
    public class CompanyController : BaseController
    {
        #region Fields

        protected readonly ICompanyService _companyService;
        protected readonly ICountryService _countryService;
        protected readonly IStateProvinceService _stateProvinceService;
        protected readonly ICurrencyService _currencyService;
        protected readonly ILanguageService _languageService;
        protected readonly IPermissionService _permissionService;
        protected readonly ILocalizationService _localizationService;
        protected readonly IEmployeeActivityService _employeeActivityService;
        protected readonly IWorkContext _workContext;
        protected readonly IMapper _mapper;

        #endregion

        #region Ctor

        public CompanyController(ICompanyService companyService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            ICurrencyService currencyService,
            ILanguageService languageService,
            IPermissionService permissionService,
            ILocalizationService localizationService,
            IEmployeeActivityService employeeActivityService,
            IWorkContext workContext,
            IMapper mapper)
        {
            _companyService = companyService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _currencyService = currencyService;
            _languageService = languageService;
            _permissionService = permissionService;
            _localizationService = localizationService;
            _employeeActivityService = employeeActivityService;
            _workContext = workContext;
            _mapper = mapper;
        }

        #endregion

        #region Actions

        public async Task<IActionResult> Edit()
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageCompany))
                return AccessDeniedPartial();

            var loggedEmployee = await _workContext.GetCurrentEmployeeAsync();
            var entity = await _companyService.GetByIdAsync(loggedEmployee.CompanyId);

            if (entity == null)
                return RedirectToAction("Index");

            var model = _mapper.Map<CompanyModel>(entity);
            await InitModelAsync(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyModel model)
        {
            if (!await _permissionService.AuthorizeAsync(PermissionProvider.ManageEmployee))
                return AccessDenied();

            if (ModelState.IsValid)
            {
                var entity = await _companyService.GetByIdAsync(model.Id);
                entity = _mapper.Map(model, entity);
                entity.Address = _mapper.Map(model.Address, entity.Address);

                await _companyService.UpdateAsync(entity);

                await _employeeActivityService.InsertAsync("Company", string.Format(await _localizationService.GetResourceAsync("Log.RecordUpdated"), entity.TradeName), entity);
                return RedirectToAction("Index", "Home");
            }

            await InitModelAsync(model);
            return View(model);
        }

        #endregion

        #region Helper

        private async Task InitModelAsync(CompanyModel model)
        {
            var countries = await _countryService.GetAllActiveAsync();
            var currencies = await _currencyService.GetAllActiveAsync();
            var languages = await _languageService.GetAllActiveAsync();

            foreach (var item in countries)
            {
                model.AvailableCountries.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.Address.CountryId
                });
            }

            if (model.Address.CountryId > 0)
            {
                var states = await _stateProvinceService.GetAllActiveByCountryAsync(model.Address.CountryId.ToInt());
                foreach (var item in states)
                {
                    model.AvailableStates.Add(new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = item.Id == model.Address.StateProvinceId
                    });
                }
            }

            foreach (var item in currencies)
            {
                model.AvailableCurrencies.Add(new SelectListItem
                {
                    Text = $"{item.Name} ({item.CurrencySymbol})",
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.CurrencyId
                });
            }

            foreach (var item in languages)
            {
                model.AvailableLanguages.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                    Selected = item.Id == model.LanguageId
                });
            }
        }

        #endregion
    }
}