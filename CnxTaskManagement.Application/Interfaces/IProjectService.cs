using CnxTaskManagement.Application.Common;
using CnxTaskManagement.Application.DTOs.Project;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Interfaces
{
    public interface IProjectService
    {
        Task<SearchResult<ProjectDto>> FindSummaryAsync(ProjectFilterDto filterDto);
        Task<ProjectDto?> GetProjectAsync(long id);
        Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto);
        Task<ProjectDto> UpdateProjectAsync(ProjectDto dto);
        Task<bool> DeleteProjectAsync(List<long> ids);
    }
}
