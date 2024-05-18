using Backlog.Web.Helpers.Attributes;
using Backlog.Web.Models.Common;

namespace Backlog.Web.Models.Masters
{
    public class SettingModel : BaseModel
    {
        [LocalizedDisplayName("SettingModel.Name")]
        public string Name { get; set; }

        [LocalizedDisplayName("SettingModel.Description")]
        public string? Description { get; set; }

        [LocalizedDisplayName("SettingModel.Value")]
        public string Value { get; set; }
    }
}