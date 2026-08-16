using UniSys.Models;

namespace UniSys.Repositories
{
    public interface ITutorRepository
    {
        Task<IEnumerable<Tutor>> GetAllTutorsAsync();
        Task<Tutor> GetTutorByIdAsync(int id);
        Task<Tutor> CreateTutorAsync(Tutor tutor);
        Task UpdateTutorAsync(Tutor tutor);
        Task DeleteTutorAsync(int id);
    }
}