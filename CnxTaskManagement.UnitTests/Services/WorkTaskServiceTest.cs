using AutoMapper;
using CnxTaskManagement.Application.Common.BaseClass;
using CnxTaskManagement.Application.DTOs.Task;
using CnxTaskManagement.Application.Services;
using CnxTaskManagement.Domain.Entities;
using CnxTaskManagement.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnxTaskManagement.UnitTests.Services
{
    public class WorkTaskServiceTest
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkTaskService _service;
        private readonly Mock<IMapper> _mockMapper;

        public WorkTaskServiceTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new ApplicationDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _service = new WorkTaskService(_mockMapper.Object, _context);

            // Seed data
            _context.Projects.Add(new Project() { Id = 1, Name = "Test Project" });
            _context.WorkTasks.Add(new Domain.Entities.WorkTask { Id = 1, Name = "Test Task", ProjectId = 1 });
            _context.SaveChanges();

            // Mock the mapping behavior
            _mockMapper.Setup(m => m.Map<WorkTaskDto>(It.IsAny<WorkTask>()))
                .Returns((WorkTask src) => src == null ? null! : new WorkTaskDto { Id = src.Id, Name = src.Name });

        }

        [Fact]
        public async Task CreateTaskAsync()
        {
            var taskDto = new WorkTaskDto { Name = "New Task2", ProjectId = 1};
            _mockMapper.Setup(m => m.Map<WorkTask>(It.IsAny<WorkTaskDto>())).Returns((WorkTaskDto src) => new WorkTask { Name = src.Name, ProjectId = src.ProjectId });
            var result = await _service.CreateTaskAsync(taskDto);
            result.Should().NotBeNull();
            _context.WorkTasks.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetTaskAsync()
        {
            // Act
            var result = await _service.GetTaskAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Test Task");
        }

        [Fact]
        public async Task FindSummary()
        {
            // Mock the mapping behavior
            _mockMapper.Setup(m => m.Map<List<WorkTaskDto>>(It.IsAny<List<WorkTask>>()))
                .Returns((List<WorkTask> src) =>
                    src == null
                        ? null!
                        : src.Select(x => new WorkTaskDto { Id = x.Id, Name = x.Name, ProjectId = x.ProjectId }).ToList());
            //Act
            var result = await _service.FindSummaryAsync(new WorkTaskFilterDto() { ProjectId = 1});

            //Assert
            result.Should().NotBeNull();
            result.data.Should().NotBeNull();
            result.data.Count.Should().Be(1);
        }

        [Fact]
        public async Task UpdateTaskAsync()
        {
            //Arrange
            var taskDto = new WorkTaskDto { Id = 1, Name = "Updated Name",ProjectId = 1 };
            //Act
            var result = await _service.UpdateTaskAsync(taskDto);
            //Assert
            result.Name.Should().BeSameAs("Updated Name");
        }

        [Fact]
        public async Task DeleteTaskAsync()
        {
            var result = await _service.DeleteTaskAsync(new List<long> { 1 });
            result.Should().BeTrue();
            _context.WorkTasks.Should().HaveCount(0);
        }
    }
}
