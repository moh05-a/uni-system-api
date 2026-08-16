using UniSys.Models;
using UniSys.Repositories;
using UniSys.Messaging;

namespace UniSys.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMessagePublisher _publisher;

        public StudentService(IStudentRepository studentRepository, IMessagePublisher publisher)
        {
            _studentRepository = studentRepository;
            _publisher = publisher;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _studentRepository.GetAllStudentsAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _studentRepository.GetStudentByIdAsync(id);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            var created = await _studentRepository.CreateStudentAsync(student);

            // Publish a domain event so other parts of the system can react.
            await _publisher.PublishAsync("student.created", new
            {
                created.Id,
                created.Name,
                created.Major,
                created.CreatedAt,
            });

            return created;
        }

        public async Task UpdateStudentAsync(Student student)
        {
            await _studentRepository.UpdateStudentAsync(student);
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteStudentAsync(id);
        }
    }
}