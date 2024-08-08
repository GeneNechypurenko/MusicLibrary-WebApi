using Microsoft.AspNetCore.Mvc;
using MusicLibrary_WebApi.Models;
using MusicLibraryApp.BLL.ModelsDTO;
using MusicLibraryApp.BLL.Services.Interfaces;

namespace MusicLibrary_WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IService<UserDTO> _userService;

		public UserController(IService<UserDTO> userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers(int pageNumber = 1, int pageSize = 10, int selected = 0)
		{
			var users = await _userService.GetAllAsync();
			var userList = users.Where(u => !u.IsAdmin).ToList();

			if (selected == 1)
			{
				userList = userList.Where(u => !u.IsAuthorized).ToList();
			}
			else if (selected == 2)
			{
				userList = userList.Where(u => u.IsBlocked).ToList();
			}

			var count = userList.Count();
			var usersOnPage = userList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			return Ok(usersOnPage);
		}


		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(int id)
		{
			var user = await _userService.GetAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
		{
			if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
			{
				return BadRequest("Invalid login details.");
			}

			var user = await _userService.GetAsync(loginModel.Username);
			if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
			{
				return Unauthorized();
			}
			return Ok(user);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationModel registrationModel)
		{
			if (registrationModel.Password != registrationModel.Confirmation)
			{
				return BadRequest("Passwords do not match.");
			}
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registrationModel.Password);
			var newUserDTO = new UserDTO
			{
				Username = registrationModel.Username,
				PasswordHash = hashedPassword,
			};
			await _userService.CreateAsync(newUserDTO);

			return CreatedAtAction(nameof(Login), new { username = newUserDTO.Username }, newUserDTO);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditUser(int id, [FromForm] IFormCollection form)
		{
			if (form == null)
			{
				return BadRequest("Invalid user data.");
			}

			var userDTO = await _userService.GetAsync(id);
			if (userDTO == null)
			{
				return NotFound();
			}

			userDTO.IsAuthorized = form["isAuthorized"] == "0";
			userDTO.IsBlocked = form["isBlocked"] == "0";

			await _userService.UpdateAsync(userDTO);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var userDTO = await _userService.GetAsync(id);
			if (userDTO == null)
			{
				return NotFound();
			}

			await _userService.DeleteAsync(id);
			return NoContent();
		}
	}
}

