using MusicLibraryApp.DAL.Models;

namespace MusicLibraryApp.DAL.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		IRepository<User> Users  { get; }
		IRepository<Category> Categories { get; }
		IRepository<Tune> Tunes { get; }
		Task SaveAsync();
	}
}
