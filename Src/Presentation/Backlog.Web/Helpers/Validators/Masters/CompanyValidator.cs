using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;
using FluentValidation;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class CompanyValidator : AbstractValidator<CompanyModel>
    {
        public CompanyValidator(ILocalizationService localizationService, ICompanyService companyService)
        {
            RuleFor(r => r.TradeName)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.TradeName.RequiredMsg"))
                .MaximumLength(750).WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.TradeName.MaxLengthMsg"));

            RuleFor(r => r.RegisteredName)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.RegisteredName.RequiredMsg"))
                .MaximumLength(750).WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.RegisteredName.MaxLengthMsg"));

            RuleFor(r => r.ContactPerson)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.ContactPerson.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.ContactPerson.MaxLengthMsg"));

            RuleFor(r => r.PhoneNumber)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.PhoneNumber.RequiredMsg"))
                .MaximumLength(20).WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.PhoneNumber.MaxLengthMsg"));

            RuleFor(r => r.Email)
                .EmailAddress().WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.Email.InvalidEmailMsg"))
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("CompanyModel.Email.MaxLengthMsg"));
        }
    }
}