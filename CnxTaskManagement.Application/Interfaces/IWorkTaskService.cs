using CnxTaskManagement.Application.Common.Utils;
using CnxTaskManagement.Application.Common;
using CnxTaskManagement.Application.DTOs.Task;
using CnxTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Interfaces
{
    public interface IWorkTaskService
    {
        Task<SearchResult<WorkTaskDto>> FindSummaryAsync(WorkTaskFilterDto filterDto);
        Task<WorkTaskDto> CreateTaskAsync(WorkTaskDto taskDto);
        Task<WorkTaskDto> UpdateTaskAsync(WorkTaskDto taskDto);
        Task<WorkTaskDto> GetTaskAsync(long id);
        Task<bool> DeleteTaskAsync(List<long> ids);
    }
}
