using Microsoft.AspNetCore.Mvc;
using UniSys.Models;
using UniSys.DTOs;
using UniSys.Repositories;
using UniSys.Filters;
namespace UniversityApi.Controllers
{
    [LoggingActionFilter]

    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentResponseDto>>> GetStudents()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            var response = students.Select(s => new StudentResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Major = s.Major,
                EnrolledSubjectNames = s.EnrolledSubjects
                    .Where(sub => sub.Name != null)
                    .Select(sub => sub.Name!)
                    .ToList()
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(StudentCreateDto dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Major = dto.Major,
                GPA = dto.GPA

            };

            var createdStudent = await _studentRepository.CreateStudentAsync(student);
            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id) return BadRequest();
            await _studentRepository.UpdateStudentAsync(student);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentRepository.DeleteStudentAsync(id);
            return NoContent();
        }
    }
}