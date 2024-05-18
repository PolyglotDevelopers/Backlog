using FluentValidation;
using Backlog.Service.Localization;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class SettingValidator : AbstractValidator<SettingModel>
    {
        public SettingValidator(ILocalizationService localizationService)
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("SettingModel.Name.RequiredMsg"))
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("SettingModel.Name.MaxLengthMsg"));   
            
            RuleFor(r => r.Description)
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("SettingModel.Description.MaxLengthMsg")); 

            RuleFor(r => r.Value)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("SettingModel.Value.RequiredMsg"))
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("SettingModel.Value.MaxLengthMsg"));         
        }
    }
}