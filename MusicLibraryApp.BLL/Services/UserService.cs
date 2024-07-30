using MusicLibraryApp.BLL.ModelsDTO;
using MusicLibraryApp.BLL.Services.Interfaces;
using MusicLibraryApp.DAL.Repositories.Interfaces;
using MusicLibraryApp.DAL.Models;
using AutoMapper;

namespace MusicLibraryApp.BLL.Services
{
	public class UserService : IService<UserDTO>
	{
		public IUnitOfWork UnitOfWork { get; set; }
		public UserService(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;
		public async Task CreateAsync(UserDTO modelDTO)
		{
			var user = new User
			{
				Id = modelDTO.Id,
				Username = modelDTO.Username,
				Password = modelDTO.Password,
				PasswordHash = modelDTO.PasswordHash,
				IsAdmin = modelDTO.IsAdmin,
				IsAuthorized = modelDTO.IsAuthorized,
				IsBlocked = modelDTO.IsBlocked,
			};
			await UnitOfWork.Users.CreateAsync(user);
			await UnitOfWork.SaveAsync();
		}

		public async Task DeleteAsync(int id)
		{
			await UnitOfWork.Users.DeleteAsync(id);
			await UnitOfWork.SaveAsync();
		}

		public async Task<IEnumerable<UserDTO>> GetAllAsync() => new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()))
			.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await UnitOfWork.Users.GetAllAsync());

		public async Task<UserDTO> GetAsync(int id)
		{
			var user = await UnitOfWork.Users.GetAsync(id);
			return new UserDTO
			{
				Id = user.Id,
				Username = user.Username,
				Password = user.Password,
				PasswordHash = user.PasswordHash,
				IsAdmin = user.IsAdmin,
				IsAuthorized = user.IsAuthorized,
				IsBlocked = user.IsBlocked,
			};
		}

		public async Task<UserDTO> GetAsync(string name)
		{
			var user = await UnitOfWork.Users.GetAsync(name);
			return new UserDTO
			{
				Id = user.Id,
				Username = user.Username,
				Password = user.Password,
				PasswordHash = user.PasswordHash,
				IsAdmin = user.IsAdmin,
				IsAuthorized = user.IsAuthorized,
				IsBlocked = user.IsBlocked,
			};
		}

		public async Task UpdateAsync(UserDTO modelDTO)
		{
			var user = await UnitOfWork.Users.GetAsync(modelDTO.Id);
			if (user != null)
			{
				user.Username = modelDTO.Username;
				user.Password = modelDTO.Password;
				user.PasswordHash = modelDTO.PasswordHash;
				user.IsAdmin = modelDTO.IsAdmin;
				user.IsAuthorized = modelDTO.IsAuthorized;
				user.IsBlocked = modelDTO.IsBlocked;

				UnitOfWork.Users.Update(user);
				await UnitOfWork.SaveAsync();
			};
		}
	}
}
