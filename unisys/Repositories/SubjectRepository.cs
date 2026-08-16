using Microsoft.EntityFrameworkCore;
using UniSys.Data;
using UniSys.Models;
using UniSys.Repositories;

namespace UniSys.Services
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Subject>> GetAllSubjectsAsync() => 
            await _context.Subjects.Include(s => s.Tutor).Include(s => s.Students).ToListAsync();

        public async Task<Subject?> GetSubjectByIdAsync(int id) => 
            await _context.Subjects
            .Include(s => s.Tutor)
            .Include(s => s.Students)
            .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<Subject> CreateSubjectAsync(Subject subject, int tutorId)
        {
            var tutor = await _context.Tutors.FindAsync(tutorId);

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            
            _context.Entry(subject).Reference(s => s.Tutor).CurrentValue = tutor;
            await _context.SaveChangesAsync();

            return subject;
        }

        public async Task EnrollStudentAsync(int subjectId, int studentId)
        {
            var subject = await _context.Subjects.Include(s => s.Students).FirstOrDefaultAsync(s => s.Id == subjectId);
            var student = await _context.Students.FindAsync(studentId);


            subject.Students.Add(student);
            await _context.SaveChangesAsync();
        }
    }
}