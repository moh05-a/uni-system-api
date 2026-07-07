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
    
    public class TutorControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TutorControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }


        [Fact]
        public async Task GetTutors_ReturnsOk_WithSeededTutors()
        {
            var response = await _client.GetAsync("/api/tutor");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var tutors = await response.Content.ReadFromJsonAsync<List<TutorResponseDto>>();
            tutors.Should().NotBeNull();
            tutors!.Count.Should().Be(2);
            tutors.Should().Contain(t => t.Name == "Dr. Smith");
            tutors.Should().Contain(t => t.Name == "Dr. Johnson");
        }


        [Fact]
        public async Task GetTutorById_ExistingId_ReturnsTutor()
        {
            var response = await _client.GetAsync("/api/tutor/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var tutor = await response.Content.ReadFromJsonAsync<TutorResponseDto>();
            tutor!.Name.Should().Be("Dr. Smith");
            tutor.Department.Should().Be("Computer Science");
        }

        [Fact]
        public async Task GetTutorById_NonExistingId_Returns404()
        {
            var response = await _client.GetAsync("/api/tutor/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task CreateTutor_ValidData_Returns201()
        {
            var newTutor = new TutorCreateDto
            {
                Name       = "Dr. Williams",
                Department = "Physics",
                Salary     = 75000
            };

            var response = await _client.PostAsJsonAsync("/api/tutor", newTutor);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<TutorResponseDto>();
            created!.Name.Should().Be("Dr. Williams");
            created.Department.Should().Be("Physics");
        }


        [Fact]
        public async Task DeleteTutor_ExistingId_Returns204()
        {
            var response = await _client.DeleteAsync("/api/tutor/2");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteTutor_NonExistingId_Returns404()
        {
            var response = await _client.DeleteAsync("/api/tutor/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
//     // ════════════════════════════════════════════════════════════════════════════
//     //  SUBJECTS
//     // ════════════════════════════════════════════════════════════════════════════
//     public class SubjectControllerTests : IClassFixture<CustomWebApplicationFactory>
//     {
//         private readonly HttpClient _client;

//         public SubjectControllerTests(CustomWebApplicationFactory factory)
//         {
//             _client = factory.CreateClient();
//         }

//         // ─── GET /api/subject ────────────────────────────────────────────────

//         [Fact]
//         public async Task GetSubjects_ReturnsOk_WithSeededSubjects()
//         {
//             var response = await _client.GetAsync("/api/subject");

//             response.StatusCode.Should().Be(HttpStatusCode.OK);

//             var subjects = await response.Content.ReadFromJsonAsync<List<SubjectResponseDto>>();
//             subjects.Should().NotBeNull();
//             subjects!.Count.Should().Be(2);
//             subjects.Should().Contain(s => s.Name == "Algorithms");
//             subjects.Should().Contain(s => s.Name == "Calculus");
//         }

//         // ─── GET /api/subject/{id} ───────────────────────────────────────────

//         [Fact]
//         public async Task GetSubjectById_ExistingId_ReturnsSubject()
//         {
//             var response = await _client.GetAsync("/api/subject/1");

//             response.StatusCode.Should().Be(HttpStatusCode.OK);

//             var subject = await response.Content.ReadFromJsonAsync<SubjectResponseDto>();
//             subject!.Name.Should().Be("Algorithms");
//             subject.CreditHours.Should().Be(3);
//         }

//         [Fact]
//         public async Task GetSubjectById_NonExistingId_Returns404()
//         {
//             var response = await _client.GetAsync("/api/subject/999");

//             response.StatusCode.Should().Be(HttpStatusCode.NotFound);
//         }

//         // ─── POST /api/subject ───────────────────────────────────────────────

//         [Fact]
//         public async Task CreateSubject_ValidTutorId_Returns201()
//         {
//             var newSubject = new SubjectCreateDto
//             {
//                 Name        = "Data Structures",
//                 CreditHours = 3,
//                 TutorId     = 1   // Dr. Smith seeded in factory
//             };

//             var response = await _client.PostAsJsonAsync("/api/subject", newSubject);

//             response.StatusCode.Should().Be(HttpStatusCode.Created);

//             var created = await response.Content.ReadFromJsonAsync<SubjectResponseDto>();
//             created!.Name.Should().Be("Data Structures");
//         }

//         [Fact]
//         public async Task CreateSubject_InvalidTutorId_ReturnsBadRequest()
//         {
//             var newSubject = new SubjectCreateDto
//             {
//                 Name        = "Ghost Subject",
//                 CreditHours = 2,
//                 TutorId     = 9999  // Tutor doesn't exist
//             };

//             var response = await _client.PostAsJsonAsync("/api/subject", newSubject);

//             response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//         }

//         // ─── POST /api/subject/{subjectId}/enroll/{studentId} ────────────────

//         [Fact]
//         public async Task EnrollStudent_ValidIds_ReturnsOk()
//         {
//             var response = await _client.PostAsync("/api/subject/1/enroll/1", null);

//             response.StatusCode.Should().Be(HttpStatusCode.OK);
//         }

//         [Fact]
//         public async Task EnrollStudent_InvalidStudentId_ReturnsBadRequest()
//         {
//             var response = await _client.PostAsync("/api/subject/1/enroll/9999", null);

//             response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//         }
//     }
// }