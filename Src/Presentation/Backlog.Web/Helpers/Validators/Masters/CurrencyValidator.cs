using FluentValidation;
using Backlog.Core.Common;
using Backlog.Service.Localization;
using Backlog.Service.Masters;
using Backlog.Web.Helpers.Extensions;
using Backlog.Web.Models.Masters;

namespace Backlog.Web.Helpers.Validators.Masters
{
    public class CurrencyValidator : AbstractValidator<CurrencyModel>
    {
        public CurrencyValidator(IWorkContext workContext, ILocalizationService localizationService, ICurrencyService currencyService)
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.Name.RequiredMsg"))
                .MaximumLength(100).WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.Name.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await currencyService.GetByNameAsync(x.Name);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await currencyService.GetByNameAsync(x.Name);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.Name.UniqueMsg"));

            RuleFor(r => r.Description)
                .MaximumLength(250).WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.Description.MaxLengthMsg"));

            RuleFor(r => r.CurrencyCode)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.CurrencyCode.RequiredMsg"))
                .MaximumLength(10).WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.CurrencyCode.MaxLengthMsg"))
                .MustAwait(async (x, context) =>
                {
                    if (x.Id > 0)
                    {
                        var editedEntity = await currencyService.GetByCurrencyCodeAsync(x.CurrencyCode);
                        return editedEntity == null || editedEntity.Id == x.Id;
                    }
                    var entity = await currencyService.GetByCurrencyCodeAsync(x.CurrencyCode);
                    return entity == null;
                }).WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.CurrencyCode.UniqueMsg"));

            RuleFor(r => r.DecimalPlace)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.DecimalPlace.RequiredMsg"));

            RuleFor(r => r.DisplayOrder)
                .NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("CurrencyModel.DisplayOrder.RequiredMsg"));
        }
    }
}