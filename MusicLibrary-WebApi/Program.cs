using MusicLibraryApp.BLL.Infrastructure;
using MusicLibraryApp.BLL.ModelsDTO;
using MusicLibraryApp.BLL.Services.Interfaces;
using MusicLibraryApp.BLL.Services;

namespace MusicLibrary_WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddApplicationDbContext(builder.Configuration.GetConnectionString("DefaultConnection")!);
			builder.Services.AddUnitOfWorkService();
			builder.Services.AddScoped<IService<UserDTO>, UserService>();
			builder.Services.AddScoped<IService<CategoryDTO>, CategoryService>();
			builder.Services.AddScoped<IService<TuneDTO>, TuneService>();

			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
			});

			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.MapControllers();

			app.Run();
		}
	}
}
