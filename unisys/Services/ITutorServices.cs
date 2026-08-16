using UniSys.Models;

namespace UniSys.Services
{
    public interface ITutorService
    {
        Task<IEnumerable<Tutor>> GetAllTutorsAsync();
        Task<Tutor> GetTutorByIdAsync(int id);
        Task<Tutor> CreateTutorAsync(Tutor tutor);
        Task UpdateTutorAsync(Tutor tutor);
        Task DeleteTutorAsync(int id);
    }
}