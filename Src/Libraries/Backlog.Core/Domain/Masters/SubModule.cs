using Backlog.Core.Domain.Common;

namespace Backlog.Core.Domain.Masters
{
    public class SubModule : BaseEntity, ISoftDeletedEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public int ModuleId { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public virtual Module Module { get; set; }
    }
}