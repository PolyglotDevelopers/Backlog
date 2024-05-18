using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.Masters
{
    public class ProjectMemberModel : BaseModel
    {
        public ProjectMemberModel()
        {
            AvailableEmployees = [];
        }

        public int ProjectId { get; set; }

        [LocalizedDisplayName("ProjectMemberModel.Employee")]
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        [LocalizedDisplayName("ProjectMemberModel.CanReport")]
        public bool CanReport { get; set; }

        [LocalizedDisplayName("ProjectMemberModel.CanClose")]
        public bool CanClose { get; set; }

        public IList<SelectListItem> AvailableEmployees { get; set; }
    }
}
