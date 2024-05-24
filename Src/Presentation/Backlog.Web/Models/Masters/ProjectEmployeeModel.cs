using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.Masters
{
    public class ProjectEmployeeModel : BaseModel
    {
        public ProjectEmployeeModel()
        {
            AvailableEmployees = [];
        }

        public int ProjectId { get; set; }

        [LocalizedDisplayName("ProjectEmployeeModel.Employee")]
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        [LocalizedDisplayName("ProjectEmployeeModel.CanReport")]
        public bool CanReport { get; set; }

        [LocalizedDisplayName("ProjectEmployeeModel.CanClose")]
        public bool CanClose { get; set; }

        public IList<SelectListItem> AvailableEmployees { get; set; }
    }
}
