using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backlog.Web.Models.WorkItems
{
    public class BacklogItemModel : BaseModel
    {
        public BacklogItemModel()
        {
            AvailableParents = [new() { Value = "0", Text = "Select" }];
            AvailableTaskTypes = [];
            AvailableSeverities = [];
            AvailableProjects = [];
            AvailableModules = [new() { Value = "0", Text = "Select" }];
            AvailableSubModules = [new() { Value = "0", Text = "Select" }];
            AvailableSprints = [new() { Value = "0", Text = "Select" }];
            AvailableAssignees = [new() { Value = "0", Text = "Select" }];
            AvailableStatus = [];
        }

        public Guid Code { get; set; }

        [LocalizedDisplayName("BacklogItem.Title")]
        public string Title { get; set; }

        [LocalizedDisplayName("BacklogItem.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("BacklogItem.Parent")]
        public int? ParentId { get; set; }

        [LocalizedDisplayName("BacklogItem.TaskType")]
        public int TaskTypeId { get; set; }

        [LocalizedDisplayName("BacklogItem.Severity")]
        public int SeverityId { get; set; }

        [LocalizedDisplayName("BacklogItem.DueDate")]
        public DateOnly? DueDate { get; set; }

        [LocalizedDisplayName("BacklogItem.Project")]
        public int ProjectId { get; set; }

        [LocalizedDisplayName("BacklogItem.Module")]
        public int? ModuleId { get; set; }

        [LocalizedDisplayName("BacklogItem.SubModule")]
        public int? SubModuleId { get; set; }

        [LocalizedDisplayName("BacklogItem.Sprint")]
        public int? SprintId { get; set; }

        [LocalizedDisplayName("BacklogItem.Assignee")]
        public int? AssigneeId { get; set; }

        public int ReOpenCount { get; set; }

        public int SubTaskCount { get; set; }

        [LocalizedDisplayName("BacklogItem.Status")]
        public int StatusId { get; set; }

        [LocalizedDisplayName("BacklogItem.Files")]
        public List<IFormFile> Files { get; set; }

        public IList<SelectListItem> AvailableParents { get; set; }

        public IList<SelectListItem> AvailableTaskTypes { get; set; }

        public IList<SelectListItem> AvailableSeverities { get; set; }

        public IList<SelectListItem> AvailableProjects { get; set; }

        public IList<SelectListItem> AvailableModules { get; set; }

        public IList<SelectListItem> AvailableSubModules { get; set; }

        public IList<SelectListItem> AvailableSprints { get; set; }

        public IList<SelectListItem> AvailableAssignees { get; set; }

        public IList<SelectListItem> AvailableStatus { get; set; }
    }

    public class BacklogItemGridModel : BaseModel
    {
        public BacklogItemGridModel()
        {
            TaskType = new TaskTypeListModel();
            Severity = new SeverityListModel();
            Status = new StatusListModel();
        }

        public string Title { get; set; }

        public DateOnly? DueDate { get; set; }

        public string Project { get; set; }

        public string? Module { get; set; }

        public string? SubModule { get; set; }

        public string? Sprint { get; set; }

        public string? Assignee { get; set; }

        public int ReOpenCount { get; set; }

        public int SubTaskCount { get; set; }

        public TaskTypeListModel TaskType { get; set; }

        public SeverityListModel Severity { get; set; }

        public StatusListModel Status { get; set; }
    }
}
