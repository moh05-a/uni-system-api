using Microsoft.AspNetCore.Mvc;
using UniSys.DTOs;
using UniSys.Models;
using UniSys.Repositories;

namespace UniSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorRepository _tutorRepository;
        public TutorController(ITutorRepository tutorRepository) => _tutorRepository = tutorRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TutorResponseDto>>> GetTutors()
        {
            var tutors = await _tutorRepository.GetAllTutorsAsync();
            var response = tutors.Select(t => new TutorResponseDto
            {
                Id = t.Id,
                Name = t.Name,
                Department = t.Department,
                SubjectNames = t.TeachingSubjects?.Select(s => s.Name!).ToList() ?? new List<string>()
            });
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TutorResponseDto>> GetTutor(int id)
        {
            var tutor = await _tutorRepository.GetTutorByIdAsync(id);
            if (tutor == null) return NotFound();

            var response = new TutorResponseDto
            {
                Id = tutor.Id,
                Name = tutor.Name,
                Department = tutor.Department,
                SubjectNames = tutor.TeachingSubjects?.Select(s => s.Name!).ToList() ?? new List<string>()
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<TutorResponseDto>> CreateTutor([FromBody] TutorCreateDto dto)
        {
            var tutor = new Tutor { Name = dto.Name, Department = dto.Department };
            var created = await _tutorRepository.CreateTutorAsync(tutor);

            var response = new TutorResponseDto { Id = created.Id, Name = created.Name, Department = created.Department };
            return CreatedAtAction(nameof(GetTutor), new { id = created.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTutor(int id)
        {
            var tutor = await _tutorRepository.GetTutorByIdAsync(id);
            if (tutor == null) return NotFound();

            await _tutorRepository.DeleteTutorAsync(id);
            return NoContent();
        }
    }
}