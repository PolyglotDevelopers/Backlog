﻿using Backlog.Core.Common;
using Backlog.Core.Domain.Employees;
using Backlog.Core.Domain.Localization;
using Backlog.Core.Domain.Masters;
using Backlog.Service.Authentication;
using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Microsoft.AspNetCore.Localization;

namespace Backlog.Web.Helpers.Common
{
    public class WorkContext : IWorkContext
    {
        #region Fields

        protected readonly ILanguageService _languageService;
        protected readonly IProjectService _projectService;
        protected readonly IAuthenticationService _authenticationService;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected Employee _cachedEmployee;
        protected Language _cachedLanguage;
        protected Currency _cachedCurrency;
        protected List<Project> _cachedProjects;

        #endregion Fields

        #region Ctor

        public WorkContext(ILanguageService languageService,
            IProjectService projectService,
            IAuthenticationService authenticationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _languageService = languageService;
            _projectService = projectService;
            _authenticationService = authenticationService;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion Ctor

        #region Methods

        public async Task<Employee> GetCurrentEmployeeAsync()
        {
            if (_cachedEmployee != null)
                return _cachedEmployee;

            await SetCurrentEmployeeAsync();

            return _cachedEmployee;
        }

        public async Task SetCurrentEmployeeAsync(Employee employee = null)
        {
            if (employee == null)
                employee = await _authenticationService.GetAuthenticatedEmployeeAsync();

            if (!employee.Deleted && employee.Status == (int)EmployeeStatusEnum.Active)
            {
                SetEmployeeCookie(employee.Code);
                _cachedEmployee = employee;
            }
        }

        public virtual async Task<Language> GetCurrentEmployeeLanguageAsync()
        {
            if (_cachedLanguage != null)
                return _cachedLanguage;

            var employee = await GetCurrentEmployeeAsync();

            var currentLanguageId = employee.LanguageId;
            var allLanguages = await _languageService.GetAllAsync();

            var detectedLanguage = allLanguages.FirstOrDefault(language => language.Id == currentLanguageId);

            SetLanguageCookie(detectedLanguage);

            _cachedLanguage = detectedLanguage;

            return _cachedLanguage;
        }

        public virtual async Task<List<Project>> GetCurrentEmployeeProjectsAsync()
        {
            if (_cachedProjects != null)
                return _cachedProjects;

            var employee = await GetCurrentEmployeeAsync();

            var accessibleProjects = await _projectService.GetAllAccessibleProjectsForEmployeeAsync(employee.Id);

            _cachedProjects = accessibleProjects.ToList();

            return _cachedProjects;
        }

        #endregion

        #region  Helper

        protected void SetEmployeeCookie(Guid code)
        {
            if (_httpContextAccessor.HttpContext?.Response?.HasStarted ?? true)
                return;

            //delete current cookie value
            var cookieName = WebConstant.EmployeeCookie;
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = WebConstant.EmployeeCookieExpires;
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (code == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate,
                // Secure = _webHelper.IsCurrentConnectionSecured()
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, code.ToString(), options);
        }

        protected virtual void SetLanguageCookie(Language language)
        {
            if (_httpContextAccessor.HttpContext?.Response?.HasStarted ?? true)
                return;

            //delete current cookie value
            var cookieName = WebConstant.CultureCookie;
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            if (string.IsNullOrEmpty(language?.LanguageCulture))
                return;

            //set new cookie value
            var value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language.LanguageCulture));
            var options = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, value, options);
        }

        #endregion
    }
}