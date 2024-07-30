using MusicLibraryApp.BLL.ModelsDTO;

namespace MusicLibraryApp.BLL.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<T> GetAsync(string name);
        Task<IEnumerable<T>> GetAllAsync();
        Task CreateAsync(T modelDTO);
        Task UpdateAsync(T modelDTO);
        Task DeleteAsync(int id);
    }
}
