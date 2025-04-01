using AutoMapper;
using CnxTaskManagement.Application.DTOs.Project;
using CnxTaskManagement.Application.DTOs.Task;
using CnxTaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WorkTask, WorkTaskDto>().ReverseMap()
                .ForMember(dest => dest.Project, opt => opt.Ignore()); // Ignore Project mapping
                //.AfterMap((src, dest) => dest.Project = new Project { Id = src.ProjectId }); // Attach existing Project
            CreateMap<Project, ProjectDto>().ReverseMap();
        }
    }
}
