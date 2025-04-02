using AutoMapper;
using CnxTaskManagement.Application.Common;
using CnxTaskManagement.Application.Common.Interfaces;
using CnxTaskManagement.Application.Common.Utils;
using CnxTaskManagement.Application.DTOs.Project;
using CnxTaskManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IApiService _apiService;

        public ProjectService(IMapper mapper, IApplicationDbContext context, IConfiguration configuration, IApiService apiService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _apiService = apiService;
        }

        public async Task<SearchResult<ProjectDto>> FindSummaryAsync(ProjectFilterDto filterDto)
        {
            var result = new SearchResult<ProjectDto>();

            int totalRecord = 0;
            var query = _context.Projects.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filterDto.Keyword))
                query = query.Where(x => x.Name.ToUpper().Contains(filterDto.Keyword.ToUpper()));

            totalRecord = await query.CountAsync();

            if (filterDto.PageSize != null && filterDto.PageNumber != null)
            {
                query = query
                    .Skip((filterDto.PageNumber.Value - 1) * filterDto.PageSize.Value)
                    .Take(filterDto.PageSize.Value)
                    .Select(t => t);
            }

            result.data = _mapper.Map<List<ProjectDto>>(await query.ToListAsync());
            result.PageNumber = filterDto.PageNumber ?? 0;
            result.PageSize = filterDto.PageSize ?? 0;
            result.TotalRecords = totalRecord;
            result.Keyword = filterDto.Keyword;
            result.TotalPages = totalRecord != 0 && filterDto.PageSize.HasValue ? (int)Math.Ceiling((double)totalRecord / filterDto.PageSize.Value) : 0;
            DateTimeConvertUtil.ConvertDateTimesToLocal(result.data);
            return result;
        }

        public async Task<ProjectDto?> GetProjectAsync(long id)
        {
            var result = await _context.Projects.Where(x => x.Id == id)
                .Include(x => x.WorkTasks)
                .FirstOrDefaultAsync();
            var resultDto = _mapper.Map<ProjectDto>(result);
            if (resultDto != null)
            {
                resultDto.WorkTasks = resultDto.WorkTasks.OrderBy(x => x.CreateDate).ToList();
                DateTimeConvertUtil.ConvertDateTimesToLocal(resultDto);
            }
            return resultDto;
        }

        public async Task<ProjectDto> CreateProjectAsync(ProjectDto projectDto)
        {
            DateTimeConvertUtil.ConvertDateTimesToUtc(projectDto);
            await ValidateCreate(projectDto);
            var project = _mapper.Map<Domain.Entities.Project>(projectDto);
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<ProjectDto>(project);
            DateTimeConvertUtil.ConvertDateTimesToLocal(result);
            return result;
        }

        public async Task<ProjectDto> UpdateProjectAsync(ProjectDto dto)
        {
            DateTimeConvertUtil.ConvertDateTimesToUtc(dto);
            var entity = await ValidateUpdate(dto);
            var currentDateTime = DateTime.UtcNow;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.DueDateTime = dto.DueDateTime;
            entity.CompleteDateTime = dto.CompleteDateTime;
            entity.LastUpdatedDate = currentDateTime;

            await _context.SaveChangesAsync();
            var result = _mapper.Map<ProjectDto>(entity);
            DateTimeConvertUtil.ConvertDateTimesToLocal(result);
            return result;
        }

        public async Task<bool> DeleteProjectAsync(List<long> ids)
        {
            var entities = await ValidateDelete(ids);

            _context.Projects.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return true;
        }

        #region Private Method
        private async Task ValidateCreate(ProjectDto dto)
        {
            if (dto == null)
                throw new Exception("Info.ProjectId is required");

            var externalApiService = new ExternalApiService(_configuration, _apiService);
            dto.Name = await externalApiService.FindBadWord(dto.Name);

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Info.Name is required");
            if (dto.Name.Length > 50)
                throw new Exception("Info.Name length more than 50");
            if (dto.Description?.Length > 1000)
                throw new Exception("Info.Description length more than 1000");
        }
        private async Task<Domain.Entities.Project> ValidateUpdate(ProjectDto dto)
        {
            await ValidateCreate(dto);
            var entity = await _context.Projects.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                throw new Exception("Target was not found");

            return entity;
        }

        private async Task<List<Domain.Entities.Project>> ValidateDelete(List<long> ids)
        {
            var entities = await _context.Projects.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (ListUtil.IsEmptyList(entities))
                throw new Exception("Target was not found");

            if (entities.Any(x => x.WorkTasks.Any()))
                throw new Exception("Target can't delete");

            return entities;
        }
        #endregion
    }
}
