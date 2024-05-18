using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class Setting : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public string Value { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}