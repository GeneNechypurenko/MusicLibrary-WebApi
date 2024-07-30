using Microsoft.Extensions.DependencyInjection;
using MusicLibraryApp.DAL.Repositories.Interfaces;
using MusicLibraryApp.DAL.Repositories;

namespace MusicLibraryApp.BLL.Infrastructure
{
	public static class UnitOfWorkServiceExtensions
	{
		public static void AddUnitOfWorkService(this IServiceCollection services) => services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}
