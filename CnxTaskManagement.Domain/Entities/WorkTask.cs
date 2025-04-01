using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Domain.Entities
{
    public class WorkTask
    {
        public long Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int? UsedHour { get; set; }
        public int? AssignedHour { get; set; }
        public DateTime? DueDateTime { get; set; }
        public DateTime? CompleteDateTime { get; set; }
        public long? AssigneeUserId { get; set; }
        public long ProjectId { get; set; }
        public long? MainTaskEntityId { get; set; }
        public Project Project { get; set; } = default!;
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        public WorkTask()
        {
        }
    }
}
