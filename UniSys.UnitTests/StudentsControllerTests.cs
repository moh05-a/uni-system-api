using Microsoft.AspNetCore.Mvc;     // Provides ASP.NET Core MVC classes like OkObjectResult and NotFoundResult.
using Moq;                          // Provides the Moq library for creating mock (fake) objects.
using UniversityApi.Controllers;    // Imports the StudentsController class being tested.
using UniSys.DTOs;                  // Imports DTO classes (not used directly in this file, but available if needed).
using UniSys.Models;                // Imports the Student model.
using UniSys.Repositories;          // Imports the IStudentRepository interface.
using Xunit;                        // Provides the xUnit testing framework and attributes like [Fact].

namespace UniSys.UnitTests
{
    // Contains unit tests for the StudentsController.
    public class StudentsControllerTests
    {
        // Mock version of the repository.
        // Instead of using the real database, we simulate its behavior.
        private readonly Mock<IStudentRepository> _mockStudentRepository;

        // Instance of the controller that will be tested.
        private readonly StudentsController _controller;

        // Constructor runs before every test.
        // It prepares the mock repository and injects it into the controller.
        public StudentsControllerTests()
        {
            // Create a fake implementation of IStudentRepository.
            _mockStudentRepository = new Mock<IStudentRepository>();

            // Pass the fake repository to the controller.
            // This is Dependency Injection, just like the real application.
            _controller = new StudentsController(_mockStudentRepository.Object);
        }

        // Marks this method as a unit test.
        [Fact]
        public async Task GetStudent_WhenStudentExists_ReturnsOkResultWithStudent()
        {
            // Arrange

            // ID of the student we want to test.
            int studentId = 99;

            // Create a fake student that the repository should return.
            var expectedStudent = new Student
            {
                Id = studentId,
                Name = "Alice Smith",
                Major = "Computer Science"
            };

            // Configure the mock repository.
            // When GetStudentByIdAsync(99) is called,
            // return the fake student instead of querying a database.
            _mockStudentRepository
                .Setup(repository => repository.GetStudentByIdAsync(studentId))
                .ReturnsAsync(expectedStudent);

            // Act

            // Call the controller action.
            var result = await _controller.GetStudent(studentId);

            // Assert

            // Verify the controller returned HTTP 200 OK.
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Verify the object inside the response is a Student.
            var returnedStudent = Assert.IsType<Student>(okResult.Value);

            // Verify the returned student's name matches what we expected.
            Assert.Equal("Alice Smith", returnedStudent.Name);
        }

        // Another unit test.
        [Fact]
        public async Task GetStudent_WhenStudentDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange

            // ID that doesn't exist.
            int nonExistentId = 404;

            // Configure the mock repository.
            // When this ID is requested, return null.
            _mockStudentRepository
                .Setup(repository => repository.GetStudentByIdAsync(nonExistentId))
                .ReturnsAsync((Student)null!);

            // Act

            // Call the controller action.
            var result = await _controller.GetStudent(nonExistentId);

            // Assert

            // Verify the controller returned HTTP 404 Not Found.
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}