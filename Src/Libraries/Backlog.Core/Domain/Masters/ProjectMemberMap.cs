using Backlog.Core.Domain.Common;
using Backlog.Core.Domain.Employees;

namespace Backlog.Core.Domain.Masters
{
    public class ProjectMemberMap : BaseEntity
    {
        public int EmployeeId { get; set; }

        public int ProjectId { get; set; }

        public bool CanReport { get; set; }

        public bool CanClose { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Project Project { get; set; }
    }
}