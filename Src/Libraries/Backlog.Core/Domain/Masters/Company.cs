using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Localization;

namespace Backlog.Core.Domain.Masters
{
    public class Company : BaseEntity
    {
        public Guid Code { get; set; }

        public string TradeName { get; set; }

        public string RegisteredName { get; set; }

        public DateTime? RegisteredOn { get; set; }

        public string? LicenseNumber { get; set; }

        public string? TaxNumber { get; set; }

        public string ContactPerson { get; set; }

        public string PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? WebSite { get; set; }

        public int AddressId { get; set; }

        public int CurrencyId { get; set; }

        public int LanguageId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual Address Address { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual Language Language { get; set; }
    }
}
