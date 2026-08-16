using Microsoft.AspNetCore.Mvc;
using UniSys.DTOs;
using UniSys.Models;
using UniSys.Repositories;
using UniSys.Services;

namespace UniSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController(
        ISubjectRepository subjectRepository,
        ITutorRepository tutorRepository,
        IStudentRepository studentRepository,
        ICacheService cache) : ControllerBase
    {
        private const string SubjectsCacheKey = "AllSubjectsList";

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectResponseDto>>> GetSubjects()
        {
            var subjects = await cache.GetOrCreateAsync(
                SubjectsCacheKey,
                () => subjectRepository.GetAllSubjectsAsync(),
                TimeSpan.FromMinutes(5));

            var response = subjects.Select(s => new SubjectResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                CreditHours = s.CreditHours,
                TutorName = s.Tutor?.Name ?? "No Tutor Assigned",
                StudentCount = s.Students?.Count ?? 0
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectResponseDto>> GetSubject(int id)
        {
            var subject = await subjectRepository.GetSubjectByIdAsync(id);
            if (subject == null) return NotFound();

            var response = new SubjectResponseDto
            {
                Id = subject.Id,
                Name = subject.Name,
                CreditHours = subject.CreditHours,
                TutorName = subject.Tutor?.Name ?? "No Tutor Assigned",
                StudentCount = subject.Students?.Count ?? 0
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<SubjectResponseDto>> CreateSubject([FromBody] SubjectCreateDto dto)
        {
            var tutor = await tutorRepository.GetTutorByIdAsync(dto.TutorId);
            if (tutor == null) return BadRequest(new { message = "Tutor not found" });

            var subject = new Subject { Name = dto.Name, CreditHours = dto.CreditHours };
            var created = await subjectRepository.CreateSubjectAsync(subject, dto.TutorId);

            var response = new SubjectResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                CreditHours = created.CreditHours,
                TutorName = created.Tutor?.Name ?? "Assigned"
            };
            return CreatedAtAction(nameof(GetSubject), new { id = created.Id }, response);
        }

        [HttpPost("{subjectId}/enroll/{studentId}")]
        public async Task<IActionResult> EnrollStudent(int subjectId, int studentId)
        {
            var student = await studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) return BadRequest(new { message = "Student not found" });

            await subjectRepository.EnrollStudentAsync(subjectId, studentId);
            return Ok(new { message = "Student successfully enrolled in course!" });
        }
    }
}
