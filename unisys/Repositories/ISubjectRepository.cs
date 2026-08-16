using UniSys.Models;

namespace UniSys.Repositories
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<Subject>> GetAllSubjectsAsync();
        Task<Subject> GetSubjectByIdAsync(int id);
        Task<Subject> CreateSubjectAsync(Subject subject, int tutorId);
        Task EnrollStudentAsync(int subjectId, int studentId);
    }
}