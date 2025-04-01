using CnxTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Infrastructure.Data.Configurations
{
    public class WorkTaskConfiguration : IEntityTypeConfiguration<WorkTask>
    {
        public void Configure(EntityTypeBuilder<WorkTask> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Project)
                .WithMany(x => x.WorkTasks)
                .HasForeignKey(x => x.ProjectId);
            builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();
        }
    }
}
