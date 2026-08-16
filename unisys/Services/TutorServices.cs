using UniSys.Models;
using UniSys.Repositories;

namespace UniSys.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository _tutorRepository;

        // Injecting our clean Tutor repository 
        public TutorService(ITutorRepository tutorRepository)
        {
            _tutorRepository = tutorRepository;
        }

        public async Task<IEnumerable<Tutor>> GetAllTutorsAsync()
        {
            return await _tutorRepository.GetAllTutorsAsync();
        }

        public async Task<Tutor> GetTutorByIdAsync(int id)
        {
            return await _tutorRepository.GetTutorByIdAsync(id);
        }

        public async Task<Tutor> CreateTutorAsync(Tutor tutor)
        {
            return await _tutorRepository.CreateTutorAsync(tutor);
        }

        public async Task UpdateTutorAsync(Tutor tutor)
        {
            await _tutorRepository.UpdateTutorAsync(tutor);
        }

        public async Task DeleteTutorAsync(int id)
        {
            await _tutorRepository.DeleteTutorAsync(id);
        }
    }
}