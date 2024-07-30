using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApp.DAL.Data;

namespace MusicLibraryApp.BLL.Infrastructure
{
	public static class ApplicationDbContextExtensions
	{
		public static void AddApplicationDbContext(this IServiceCollection services, string connection) 
			=> services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connection));
    }
}
