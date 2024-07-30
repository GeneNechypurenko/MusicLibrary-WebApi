using AutoMapper;
using MusicLibraryApp.BLL.ModelsDTO;
using MusicLibraryApp.BLL.Services.Interfaces;
using MusicLibraryApp.DAL.Models;
using MusicLibraryApp.DAL.Repositories.Interfaces;

namespace MusicLibraryApp.BLL.Services
{
	public class CategoryService : IService<CategoryDTO>
	{
		public IUnitOfWork UnitOfWork { get; set; }
		public CategoryService(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;
		public async Task CreateAsync(CategoryDTO modelDTO)
		{
			var category = new Category
			{
				Genre = modelDTO.Genre,
				Tunes = new List<Tune>(),
			};
			await UnitOfWork.Categories.CreateAsync(category);
			await UnitOfWork.SaveAsync();
		}

		public async Task DeleteAsync(int id)
		{
			await UnitOfWork.Categories.DeleteAsync(id);
			await UnitOfWork.SaveAsync();
		}

		public async Task<IEnumerable<CategoryDTO>> GetAllAsync() => new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDTO>()))
			.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(await UnitOfWork.Categories.GetAllAsync());

		public async Task<CategoryDTO> GetAsync(int id)
		{
			var category = await UnitOfWork.Categories.GetAsync(id);
			return new CategoryDTO
			{
				Id = category.Id,
				Genre = category.Genre,
				Tunes = category.Tunes?.ToList(),
			};
		}

		public async Task<CategoryDTO> GetAsync(string name)
		{
			var category = await UnitOfWork.Categories.GetAsync(name);
			return new CategoryDTO
			{
				Id = category.Id,
				Genre = category.Genre,
				Tunes = category.Tunes?.ToList(),
			};
		}

		public async Task UpdateAsync(CategoryDTO modelDTO)
		{
			var category = await UnitOfWork.Categories.GetAsync(modelDTO.Id);
			if (category != null)
			{
				category.Genre = modelDTO.Genre;
				UnitOfWork.Categories.Update(category);
				await UnitOfWork.SaveAsync();
			}
		}
	}
}
