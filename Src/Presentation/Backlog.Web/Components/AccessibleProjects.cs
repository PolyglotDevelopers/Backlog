using AutoMapper;
using Backlog.Core.Common;
using Backlog.Web.Models.Masters;
using Microsoft.AspNetCore.Mvc;

namespace Backlog.Web.Components
{
    public class AccessibleProjects : ViewComponent
    {
        private readonly IWorkContext _workContext;
        private readonly IMapper _mapper;

        public AccessibleProjects(IWorkContext workContext,
            IMapper mapper)
        {
            _workContext = workContext;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var projects = await _workContext.GetCurrentEmployeeProjectsAsync();
            var model = projects.Select(x => _mapper.Map<AccessibleEmployeeProjectModel>(x)).ToList();

            return View(model);
        }
    }
}