using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.WorkItems
{
    public class BacklogItemModel : BaseModel
    {
        public Guid Code { get; set; }

        [LocalizedDisplayName("BacklogItem.Title")]
        public string Title { get; set; }

        [LocalizedDisplayName("BacklogItem.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("BacklogItem.Parent")]
        public int? ParentId { get; set; }

        [LocalizedDisplayName("BacklogItem.TaskType")]
        public int TaskTypeId { get; set; }

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
    }

    public class BacklogItemGridModel : BaseModel
    {
        public string Title { get; set; }

        public string TaskType { get; set; }

        public DateOnly? DueDate { get; set; }

        public string Project { get; set; }

        public string? Module { get; set; }

        public string? SubModule { get; set; }

        public string? Sprint { get; set; }

        public string? Assignee { get; set; }

        public int ReOpenCount { get; set; }

        public int SubTaskCount { get; set; }

        public string Status { get; set; }
    }
}
