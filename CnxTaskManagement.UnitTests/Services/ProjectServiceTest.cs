using AutoMapper;
using CnxTaskManagement.Application.Common.Interfaces;
using CnxTaskManagement.Application.DTOs.Project;
using CnxTaskManagement.Application.Services;
using CnxTaskManagement.Domain.Entities;
using CnxTaskManagement.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.UnitTests.Services
{
    public class ProjectServiceTest
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IApiService> _mockApiService;

        public ProjectServiceTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new ApplicationDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockApiService = new Mock<IApiService>();
            _service = new ProjectService(_mockMapper.Object,_context,_mockConfiguration.Object,_mockApiService.Object);

            // Seed data
            _context.Projects.Add(new Project() { Id = 1, Name = "Test Project" });
            _context.SaveChanges();

            // Mock the mapping behavior
            _mockMapper.Setup(m => m.Map<ProjectDto>(It.IsAny<Project>()))
                .Returns((Project src) => src == null ? null! : new ProjectDto { Id = src.Id, Name = src.Name });
            _mockConfiguration.Setup(x => x["BadwordApiKey"]).Returns("BWKey");
            
        }

        [Fact]
        public async Task CreateTaskAsync()
        {
            _mockApiService.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync("{\"censored_content\": \"New Project2\"}");

            var projectDto = new ProjectDto { Name = "New Project2"};
            _mockMapper.Setup(m => m.Map<Project>(It.IsAny<ProjectDto>())).Returns((ProjectDto src) => new Project { Name = src.Name });
            var result = await _service.CreateProjectAsync(projectDto);
            result.Should().NotBeNull();
            _context.Projects.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetTaskAsync()
        {
            // Act
            var result = await _service.GetProjectAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Test Project");
        }

        [Fact]
        public async Task FindSummary()
        {
            // Mock the mapping behavior
            _mockMapper.Setup(m => m.Map<List<ProjectDto>>(It.IsAny<List<Project>>()))
                .Returns((List<Project> src) =>
                    src == null
                        ? null!
                        : src.Select(x => new ProjectDto { Id = x.Id, Name = x.Name}).ToList());
            //Act
            var result = await _service.FindSummaryAsync(new ProjectFilterDto());

            //Assert
            result.Should().NotBeNull();
            result.data.Should().NotBeNull();
            result.data.Count.Should().Be(1);
        }

        [Fact]
        public async Task UpdateTaskAsync()
        {
            //mock Api response
            _mockApiService.Setup(x => x.SendRequestAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<object>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync("{\"censored_content\": \"Updated Name\"}");
            //Arrange
            var projectDto = new ProjectDto { Id = 1, Name = "Updated Name" };
            //Act
            var result = await _service.UpdateProjectAsync(projectDto);
            //Assert
            result.Name.Should().Be("Updated Name");
        }

        [Fact]
        public async Task DeleteTaskAsync()
        {
            //Act
            var result = await _service.DeleteProjectAsync(new List<long> { 1 });
            //Assert
            result.Should().BeTrue();
            _context.Projects.Should().HaveCount(0);
        }
    }
}
