using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using UniSys.DTOs;

namespace UniSys.IntegrationTests
{
    public class StudentsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public StudentsControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task GetStudents_ReturnsOk_WithSeededStudents()
        {
            var response = await _client.GetAsync("/api/students");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var students = await response.Content.ReadFromJsonAsync<List<StudentResponseDto>>();
            students.Should().NotBeNull();
            students!.Count.Should().Be(3);
            students.Should().Contain(s => s.Name == "Alice");
            students.Should().Contain(s => s.Name == "Bob");
        }


        [Fact]
        public async Task GetStudentById_ExistingId_ReturnsStudent()
        {
            var response = await _client.GetAsync("/api/students/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetStudentById_NonExistingId_Returns404()
        {
            var response = await _client.GetAsync("/api/students/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task CreateStudent_ValidData_Returns201()
        {
            var newStudent = new StudentCreateDto
            {
                Name  = "Diana",
                Major = "Physics"
            };

            var response = await _client.PostAsJsonAsync("/api/students", newStudent);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateStudent_ThenGetAll_NewStudentAppears()
        {
            var newStudent = new StudentCreateDto { Name = "Eve", Major = "Biology" };
            await _client.PostAsJsonAsync("/api/students", newStudent);

            var response  = await _client.GetAsync("/api/students");
            var students  = await response.Content.ReadFromJsonAsync<List<StudentResponseDto>>();

            students.Should().Contain(s => s.Name == "Eve");
        }


        [Fact]
        public async Task DeleteStudent_ExistingId_Returns204()
        {
            var response = await _client.DeleteAsync("/api/students/3");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}