using Microsoft.EntityFrameworkCore;
using MusicLibraryApp.DAL.Data;
using MusicLibraryApp.DAL.Models;
using MusicLibraryApp.DAL.Repositories.Interfaces;

namespace MusicLibraryApp.DAL.Repositories
{
    public class TuneRepository : IRepository<Tune>
    {
        private readonly ApplicationDbContext _context;

        public TuneRepository(ApplicationDbContext context) => _context = context;

        public async Task CreateAsync(Tune entity) => await _context.Tunes.AddAsync(entity);

        public async Task DeleteAsync(int id)
        {
            var tune = await _context.Tunes.FindAsync(id);
            if (tune != null) _context.Tunes.Remove(tune);
        }

        public async Task<IEnumerable<Tune>> GetAllAsync() => await _context.Tunes.Include(o => o.Category).ToListAsync();

        public async Task<Tune> GetAsync(int id) => await _context.Tunes.FindAsync(id);

        public async Task<Tune> GetAsync(string name) => await _context.Tunes.Where(o => o.Title == name).FirstOrDefaultAsync();

        public void Update(Tune entity) => _context.Tunes.Update(entity);
    }
}
