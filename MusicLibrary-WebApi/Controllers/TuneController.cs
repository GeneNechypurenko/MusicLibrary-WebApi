using Microsoft.AspNetCore.Mvc;
using MusicLibraryApp.BLL.ModelsDTO;
using MusicLibraryApp.BLL.Services.Interfaces;
using MusicLibrary_WebApi.Models;

namespace MusicLibrary_WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TuneController : ControllerBase
	{
		private readonly IService<TuneDTO> _tuneService;

		public TuneController(IService<TuneDTO> tuneService)
		{
			_tuneService = tuneService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTunes()
		{
			var tunes = await _tuneService.GetAllAsync();
			return Ok(tunes);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetTune(int id)
		{
			var tune = await _tuneService.GetAsync(id);
			if (tune == null)
			{
				return NotFound();
			}
			return Ok(tune);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTune([FromBody] TuneModel tuneModel)
		{
			if (tuneModel == null)
			{
				return BadRequest();
			}

			var tuneDTO = new TuneDTO
			{
				Performer = tuneModel.Performer,
				Title = tuneModel.Title,
				IsAuthorized = tuneModel.IsAuthorized,
				IsBlocked = tuneModel.IsBlocked,
				CategoryId = tuneModel.CategoryId
			};

			await _tuneService.CreateAsync(tuneDTO);

			var createdTune = await _tuneService.GetAsync(tuneDTO.Title);
			return CreatedAtAction(nameof(GetTune), new { id = createdTune.Id }, createdTune);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTune(int id, [FromBody] TuneModel tuneModel)
		{
			if (tuneModel == null)
			{
				return BadRequest();
			}

			var tuneDTO = new TuneDTO
			{
				Id = id,
				Performer = tuneModel.Performer,
				Title = tuneModel.Title,
				IsAuthorized = tuneModel.IsAuthorized,
				IsBlocked = tuneModel.IsBlocked,
				CategoryId = tuneModel.CategoryId
			};

			await _tuneService.UpdateAsync(tuneDTO);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTune(int id)
		{
			await _tuneService.DeleteAsync(id);
			return NoContent();
		}
	}
}
