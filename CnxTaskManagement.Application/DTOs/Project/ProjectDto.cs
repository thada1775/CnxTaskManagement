using CnxTaskManagement.Application.Common.BaseClass;
using CnxTaskManagement.Application.DTOs.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.DTOs.Project
{
    public class ProjectDto : BaseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime? DueDateTime { get; set; }
        public DateTime? CompleteDateTime { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public ICollection<WorkTaskDto> WorkTasks { get; set; }

        public ProjectDto()
        {
            WorkTasks = new List<WorkTaskDto>();
        }
    }
}
