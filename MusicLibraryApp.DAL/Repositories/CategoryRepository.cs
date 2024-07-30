using Microsoft.EntityFrameworkCore;
using MusicLibraryApp.DAL.Data;
using MusicLibraryApp.DAL.Models;
using MusicLibraryApp.DAL.Repositories.Interfaces;

namespace MusicLibraryApp.DAL.Repositories
{
	public class CategoryRepository : IRepository<Category>
	{
		private readonly ApplicationDbContext _context;

		public CategoryRepository(ApplicationDbContext context) => _context = context;

		public async Task CreateAsync(Category entity) => await _context.Categories.AddAsync(entity);

		public async Task DeleteAsync(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category != null) _context.Categories.Remove(category);
		}

		public async Task<IEnumerable<Category>> GetAllAsync() => await _context.Categories.Include(o => o.Tunes).ToListAsync();

		public async Task<Category> GetAsync(int id) => await _context.Categories.FindAsync(id);

		public async Task<Category> GetAsync(string name) => await _context.Categories.Where(o => o.Genre == name).FirstOrDefaultAsync();

		public void Update(Category entity) => _context.Categories.Update(entity);
	}
}
