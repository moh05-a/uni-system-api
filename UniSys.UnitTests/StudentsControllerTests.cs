using Microsoft.AspNetCore.Mvc;
using Moq;
using UniversityApi.Controllers;
using UniSys.DTOs;
using UniSys.Models;
using UniSys.Repositories;
using Xunit;

namespace UniSys.UnitTests
{
    public class StudentsControllerTests
    {
        private readonly Mock<IStudentRepository> _mockStudentRepository;
        private readonly StudentsController _controller;

        public StudentsControllerTests()
        {
            _mockStudentRepository = new Mock<IStudentRepository>();
            _controller = new StudentsController(_mockStudentRepository.Object);
        }

        [Fact]
        public async Task GetStudent_WhenStudentExists_ReturnsOkResultWithStudent()
        {
            int studentId = 99;
            var expectedStudent = new Student { Id = studentId, Name = "Alice Smith", Major = "Computer Science" };
            
            _mockStudentRepository.Setup(repository => repository.GetStudentByIdAsync(studentId))
                                  .ReturnsAsync(expectedStudent);

            var result = await _controller.GetStudent(studentId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedStudent = Assert.IsType<Student>(okResult.Value);
            Assert.Equal("Alice Smith", returnedStudent.Name);
        }

        [Fact]
        public async Task GetStudent_WhenStudentDoesNotExist_ReturnsNotFoundResult()
        {
            int nonExistentId = 404;
            _mockStudentRepository.Setup(repository => repository.GetStudentByIdAsync(nonExistentId))
                                  .ReturnsAsync((Student)null!);

            var result = await _controller.GetStudent(nonExistentId);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}






