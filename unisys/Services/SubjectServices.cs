using UniSys.Models;
using UniSys.Repositories; 

namespace UniSys.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IStudentRepository _studentRepository;

        public SubjectService(
            ISubjectRepository subjectRepository, 
            ITutorRepository tutorRepository, 
            IStudentRepository studentRepository)
        {
            _subjectRepository = subjectRepository;
            _tutorRepository = tutorRepository;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync()
        {
            return await _subjectRepository.GetAllSubjectsAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(int id)
        {
            var subject = await _subjectRepository.GetSubjectByIdAsync(id);
            
            return subject;
        }

        public async Task<Subject> CreateSubjectAsync(Subject subject, int tutorId)
        {
            var tutor = await _tutorRepository.GetTutorByIdAsync(tutorId);
            if (tutor == null) throw new Exception("Tutor not found");

            return await _subjectRepository.CreateSubjectAsync(subject, tutorId);
        }

        public async Task EnrollStudentAsync(int subjectId, int studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null) throw new Exception("Student not found");

            await _subjectRepository.EnrollStudentAsync(subjectId, studentId);
        }
    }
}