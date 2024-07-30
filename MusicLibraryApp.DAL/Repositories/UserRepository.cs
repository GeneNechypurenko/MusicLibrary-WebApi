using MusicLibraryApp.DAL.Repositories.Interfaces;
using MusicLibraryApp.DAL.Models;
using MusicLibraryApp.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace MusicLibraryApp.DAL.Repositories
{
	public class UserRepository : IRepository<User>
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context) => _context = context;

		public async Task CreateAsync(User entity) => await _context.AddAsync(entity);

		public async Task DeleteAsync(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user != null) _context.Users.Remove(user);
		}

		public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

		public async Task<User> GetAsync(int id) => await _context.Users.FindAsync(id);

		public async Task<User> GetAsync(string name) => await _context.Users.Where(u => u.Username == name).FirstOrDefaultAsync();

		public void Update(User entity) => _context.Users.Update(entity);
	}
}
