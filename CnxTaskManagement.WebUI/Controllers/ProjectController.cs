using CnxTaskManagement.Application.DTOs.Project;
using CnxTaskManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CnxTaskManagement.WebUI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;
        public ProjectController(IProjectService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(ProjectFilterDto filterDto)
        {
            if (!filterDto.PageNumber.HasValue || !filterDto.PageSize.HasValue)
            {
                filterDto.PageNumber = 1;
                filterDto.PageSize = 10;
            }
            var result = await _service.FindSummaryAsync(filterDto);
            return View(result);
        }

        public async Task<IActionResult> View(long id)
        {
            var result = await _service.GetProjectAsync(id);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectDto dto)
        {
            var response = await _service.CreateProjectAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(long id)
        {
            var response = await _service.GetProjectAsync(id);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectDto dto)
        {
            var response = await _service.UpdateProjectAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long id)
        {
            var response = await _service.GetProjectAsync(id);
            return View(response);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var response = await _service.DeleteProjectAsync(new List<long>() { id });
            return RedirectToAction(nameof(Index));
        }
    }
}
