using CnxTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<WorkTask> WorkTasks { get; set; }
        DbSet<Project> Projects { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
