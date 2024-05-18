using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class CurrencyModel : BaseModel
    {
        [LocalizedDisplayName("CurrencyModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("CurrencyModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("CurrencyModel.CurrencyCode")]
        public string CurrencyCode { get; set; }

        [LocalizedDisplayName("CurrencyModel.CurrencySymbol")]
        public string CurrencySymbol { get; set; }

        [LocalizedDisplayName("CurrencyModel.DecimalPlace")]
        public int DecimalPlace { get; set; }

        [LocalizedDisplayName("CurrencyModel.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LocalizedDisplayName("CurrencyModel.Active")]
        public bool Active { get; set; }
    }
}
