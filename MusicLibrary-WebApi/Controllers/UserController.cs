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
		public async Task<IActionResult> GetAllUsers()
		{
			var users = await _userService.GetAllAsync();
			return Ok(users);
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
		public async Task<IActionResult> EditUser(int id, [FromBody] UserModel userModel)
		{
			if (userModel == null)
			{
				return BadRequest("Invalid user data.");
			}

			var userDTO = await _userService.GetAsync(id);
			if (userDTO == null)
			{
				return NotFound();
			}

			userDTO.Username = userModel.Username;
			userDTO.IsAuthorized = userModel.IsAuthorized;
			userDTO.IsBlocked = userModel.IsBlocked;

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
