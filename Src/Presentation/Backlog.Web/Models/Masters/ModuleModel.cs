using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.Masters
{
    public class ModuleModel : BaseModel
    {
        public ModuleModel()
        {
            AvailableProjects =
            [
                new () { Value = "0", Text = "All projects" }
            ];
        }

        [LocalizedDisplayName("ModuleModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("ModuleModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("ModuleModel.Project")]
        public int? ProjectId { get; set; }

        [LocalizedDisplayName("ModuleModel.Active")]
        public bool Active { get; set; }

        public string ProjectName { get; set; }

        public IList<SelectListItem> AvailableProjects { get; set; }
    }
}
