using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Domain.Entities
{
    public class Project
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? DueDateTime { get; set; }
        public DateTime? CompleteDateTime { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public ICollection<WorkTask> WorkTasks { get; set; }

        public Project()
        {
            WorkTasks = new List<WorkTask>();
        }
    }
}
