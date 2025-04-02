using CnxTaskManagement.Application.Common.Interfaces;
using CnxTaskManagement.Application.Interfaces;
using CnxTaskManagement.Application.Services;
using CnxTaskManagement.Infrastructure.Data;
using CnxTaskManagement.Infrastructure.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IWorkTaskService, WorkTaskService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddHttpClient<IApiService, ApiService>();
            //services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
