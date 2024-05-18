using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencySymbol { get; set; }

        public int DecimalPlace { get; set; }

        public int DisplayOrder { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
