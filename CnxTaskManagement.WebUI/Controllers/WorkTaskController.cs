using CnxTaskManagement.Application.DTOs.Task;
using CnxTaskManagement.Application.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace CnxTaskManagement.WebUI.Controllers
{
    public class WorkTaskController : Controller
    {
        private readonly IWorkTaskService _service;
        private readonly IProjectService _projectService;
        public WorkTaskController(IWorkTaskService service, IProjectService projectService)
        {
            _projectService = projectService;
            _service = service;
        }
        public async Task<IActionResult> Index(long id)
        {
            var cuurentProject = await _projectService.GetProjectAsync(id);
            ViewData["ProjectId"] = cuurentProject;
            var result = await _service.FindSummaryAsync(new WorkTaskFilterDto()
            {
                ProjectId = id
            });
            return View(result);
        }

        public async Task<IActionResult> View(long id)
        {
            var result = await _service.GetTaskAsync(id);
            return View(result);
        }

        public IActionResult Create(long id)
        {
            ViewData["ProjectId"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkTaskDto dto)
        {
            var response = await _service.CreateTaskAsync(dto);
            return RedirectToAction(nameof(Index), new {Id = dto.ProjectId});
        }

        public async Task<IActionResult> Edit(long id)
        {
            var response = await _service.GetTaskAsync(id);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkTaskDto dto)
        {
            var response = await _service.UpdateTaskAsync(dto);
            return RedirectToAction(nameof(Index), new { Id = dto.ProjectId });
        }

        public async Task<IActionResult> Delete(long id)
        {
            var response = await _service.GetTaskAsync(id);
            return View(response);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var task = await _service.GetTaskAsync(id);
            var response = await _service.DeleteTaskAsync(new List<long>() { id });
            return RedirectToAction(nameof(Index), new { Id = task?.ProjectId });
        }
    }
}
