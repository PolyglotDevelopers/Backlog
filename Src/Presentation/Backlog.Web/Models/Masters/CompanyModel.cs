using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Backlog.Web.Models.Masters
{
    public class CompanyModel : BaseModel
    {
        public CompanyModel()
        {
            Address = new AddressModel();
            AvailableCountries = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableStates = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableCurrencies = [new SelectListItem { Text = "Select", Value = "" }];
            AvailableLanguages = [new SelectListItem { Text = "Select", Value = "" }];
        }

        [LocalizedDisplayName("CompanyModel.TradeName")]
        public string TradeName { get; set; }

        [LocalizedDisplayName("CompanyModel.RegisteredName")]
        public string RegisteredName { get; set; }

        [LocalizedDisplayName("CompanyModel.RegisteredOn")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? RegisteredOn { get; set; }

        [LocalizedDisplayName("CompanyModel.LicenseNumber")]
        public string? LicenseNumber { get; set; }

        [LocalizedDisplayName("CompanyModel.TaxNumber")]
        public string? TaxNumber { get; set; }

        [LocalizedDisplayName("CompanyModel.ContactPerson")]
        public string ContactPerson { get; set; }

        [LocalizedDisplayName("CompanyModel.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [LocalizedDisplayName("CompanyModel.Email")]
        public string? Email { get; set; }

        [LocalizedDisplayName("CompanyModel.WebSite")]
        public string? Website { get; set; }

        public AddressModel Address { get; set; }

        [LocalizedDisplayName("CompanyModel.Currency")]
        public int CurrencyId { get; set; }

        [LocalizedDisplayName("CompanyModel.Language")]
        public int LanguageId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public IList<SelectListItem> AvailableStates { get; set; }

        public List<SelectListItem> AvailableCurrencies { get; set; }

        public List<SelectListItem> AvailableLanguages { get; set; }
    }
}
