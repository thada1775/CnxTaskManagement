using CnxTaskManagement.Application.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.DTOs.Task
{
    public class WorkTaskFilterDto : BaseFilterDto
    {
        public long? ProjectId { get; set; }
    }
}
