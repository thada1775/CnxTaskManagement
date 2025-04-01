using AutoMapper;
using CnxTaskManagement.Application.Common.BaseClass;
using CnxTaskManagement.Application.Common.Utils;
using CnxTaskManagement.Application.Common;
using CnxTaskManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CnxTaskManagement.Application.Common.Interfaces;
using CnxTaskManagement.Application.DTOs.Task;
using Microsoft.EntityFrameworkCore;
using CnxTaskManagement.Domain.Entities;

namespace CnxTaskManagement.Application.Services
{
    public class WorkTaskService : IWorkTaskService
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public WorkTaskService(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<SearchResult<WorkTaskDto>> FindSummaryAsync(WorkTaskFilterDto filterDto)
        {
            var result = new SearchResult<WorkTaskDto>();

            int totalRecord = 0;
            var query = _context.WorkTasks.AsNoTracking();

            if (filterDto.ProjectId.HasValue)
                query = query.Where(x => x.ProjectId == filterDto.ProjectId.Value);

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

            result.data = _mapper.Map<List<WorkTaskDto>>(await query.ToListAsync());
            result.TotalRecords = totalRecord;
            result.TotalPages = totalRecord != 0 && filterDto.PageSize.HasValue ? (int)Math.Ceiling((double)totalRecord / filterDto.PageSize.Value) : 0;
            return result;
        }

        public async Task<WorkTaskDto> CreateTaskAsync(WorkTaskDto taskDto)
        {
            await ValidateCreate(taskDto);

            var currentDateTime = DateTime.UtcNow;
            var entity = _mapper.Map<WorkTask>(taskDto);

            entity.Id = 0;
            entity.CreateDate = currentDateTime;
            entity.LastUpdatedDate = currentDateTime;

            await _context.WorkTasks.AddAsync(entity);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<WorkTaskDto>(entity);
            return result;
        }

        public async Task<WorkTaskDto> UpdateTaskAsync(WorkTaskDto taskDto)
        {
            var entity = await ValidateUpdate(taskDto);
            var currentDateTime = DateTime.UtcNow;

            entity.Name = taskDto.Name;
            entity.Description = taskDto.Description;
            entity.MainTaskEntityId = taskDto.MainTaskEntityId;
            entity.DueDateTime = taskDto.DueDateTime;
            entity.CompleteDateTime = taskDto.CompleteDateTime;
            entity.AssigneeUserId = taskDto.AssigneeUserId;
            entity.LastUpdatedDate = currentDateTime;

            await _context.SaveChangesAsync();

            var result = _mapper.Map<WorkTaskDto>(entity);
            return result;
        }

        public async Task<WorkTaskDto> GetTaskAsync(long id)
        {
            var task = await _context.WorkTasks.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == id);
            var taskDto = _mapper.Map<WorkTaskDto>(task);
            return taskDto;
        }

        public async Task<bool> DeleteTaskAsync(List<long> ids)
        {
            var tasks = await ValidateDelete(ids);
            if (ListUtil.IsEmptyList(tasks))
                throw new ArgumentNullException(nameof(tasks));

            _context.WorkTasks.RemoveRange(tasks);
            await _context.SaveChangesAsync();
            return true;
        }

        #region Private Method
        private async System.Threading.Tasks.Task ValidateCreate(WorkTaskDto taskDto)
        {
            if (taskDto == null)
                throw new Exception("Info.ProjectId is required");

            if (string.IsNullOrWhiteSpace(taskDto.Name))
                throw new Exception("Info.Name is required");
            if (taskDto.Name.Length > 50)
                throw new Exception("Info.Name length more than 50");
            if (taskDto.Description?.Length > 1000)
                throw new Exception("Info.Description length more than 1000");

            if (!(await _context.Projects.AnyAsync(x => x.Id == taskDto.ProjectId)))
                throw new Exception("Project is invalid");
        }

        private async Task<WorkTask> ValidateUpdate(WorkTaskDto taskDto)
        {
            await ValidateCreate(taskDto);

            var entity = await _context.WorkTasks.FirstOrDefaultAsync(x => x.Id == taskDto.Id);
            if (entity == null)
                throw new Exception("Target was not found");

            return entity;
        }

        private async Task<List<WorkTask>> ValidateDelete(List<long> ids)
        {
            var entities = await _context.WorkTasks.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (ListUtil.IsEmptyList(entities))
                throw new Exception("Target was not found");

            return entities;
        }
        #endregion
    }
}
