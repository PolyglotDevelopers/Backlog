using Backlog.Core.Common;
using Backlog.Service.Employees;
using Backlog.Service.Localization;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.WorkItems;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.WorkItems
{
    public class BacklogItemValidator : AbstractValidator<BacklogItemModel>
    {
        public BacklogItemValidator(IWorkContext workContext, ILocalizationService localizationService, IEmployeeService employeeService)
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Title.RequiredMsg"))
                .MaximumLength(500).WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.Title.MaxLengthMsg"));

            RuleFor(r => r.TaskTypeId)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.TaskType.RequiredMsg"))
                .GreaterThan(0).WithMessageAwait(localizationService.GetResourceAsync("BacklogItemModel.TaskType.RequiredMsg"));
        }
    }
}