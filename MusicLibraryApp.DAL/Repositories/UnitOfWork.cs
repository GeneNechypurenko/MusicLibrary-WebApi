using MusicLibraryApp.DAL.Data;
using MusicLibraryApp.DAL.Models;
using MusicLibraryApp.DAL.Repositories.Interfaces;

namespace MusicLibraryApp.DAL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private UserRepository _userRepository;
		private CategoryRepository _categoryRepository;
		private TuneRepository _tuneRepository;

		public UnitOfWork(ApplicationDbContext context) => _context = context;

		public IRepository<User> Users => _userRepository ??= new UserRepository(_context);

		public IRepository<Category> Categories => _categoryRepository ??= new CategoryRepository(_context);

		public IRepository<Tune> Tunes => _tuneRepository ??= new TuneRepository(_context);

		public async Task SaveAsync() => await _context.SaveChangesAsync();
	}
}
