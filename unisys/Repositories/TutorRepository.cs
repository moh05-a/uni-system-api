using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UniSys.Data;
using UniSys.Models;

namespace UniSys.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private readonly ApplicationDbContext _context;

        public TutorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tutor>> GetAllTutorsAsync()
        {
            // Includes the list of subjects this tutor teaches
            return await _context.Tutors
                .Include(t => t.TeachingSubjects)
                .ToListAsync();
        }

        public async Task<Tutor?> GetTutorByIdAsync(int id)
        {
            return await _context.Tutors
                .Include(t => t.TeachingSubjects)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tutor> CreateTutorAsync(Tutor tutor)
        {
            _context.Tutors.Add(tutor);
            await _context.SaveChangesAsync();
            return tutor;
        }

        public async Task UpdateTutorAsync(Tutor tutor)
        {
            _context.Entry(tutor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTutorAsync(int id)
        {
            var tutor = await _context.Tutors.FindAsync(id);
            if (tutor != null)
            {
                _context.Tutors.Remove(tutor);
                await _context.SaveChangesAsync();
            }
        }
    }
}