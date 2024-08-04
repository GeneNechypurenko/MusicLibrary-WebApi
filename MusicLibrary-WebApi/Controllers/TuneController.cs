﻿using Microsoft.AspNetCore.Mvc;
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
		private readonly IWebHostEnvironment _web;

		public TuneController(IService<TuneDTO> tuneService, IWebHostEnvironment web)
		{
			_tuneService = tuneService;
			_web = web;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTunes(int selected = 0, int pageNumber = 1, int pageSize = 5)
		{
			var tunes = await _tuneService.GetAllAsync();

			switch (selected)
			{
				case -2:
					tunes = tunes.Where(t => !t.IsAuthorized).ToList();
					break;
				case -1:
					tunes = tunes.Where(t => t.IsBlocked).ToList();
					break;
				case 0:
					tunes = tunes.Where(t => t.IsAuthorized && !t.IsBlocked).ToList();
					break;
				default:
					tunes = tunes.Where(t => t.CategoryId == selected && !t.IsBlocked && t.IsAuthorized).ToList();
					break;
			}

			var count = tunes.Count();
			var tunesOnPage = tunes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			var pagination = new PaginationModel(count, pageNumber, pageSize);

			return Ok(new { tunes = tunesOnPage, pagination });
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
		public async Task<IActionResult> CreateTune([FromForm] CreateTuneModel model)
		{
			if (model.File == null || model.Poster == null)
			{
				return BadRequest("Files are required.");
			}

			string tuneFilePath = await FileUpload(model.File, "res/Tunes/Upload");
			string posterFilePath = await FileUpload(model.Poster, "res/Tunes/Posters");

			var tuneDTO = new TuneDTO
			{
				Performer = model.Performer,
				Title = model.Title,
				FileUrl = tuneFilePath,
				PosterUrl = posterFilePath,
				CategoryId = model.CategoryId,
				IsAuthorized = true,
				IsBlocked = false
			};

			await _tuneService.CreateAsync(tuneDTO);

			var createdTune = await _tuneService.GetAsync(tuneDTO.Title);
			return CreatedAtAction(nameof(GetTune), new { id = createdTune.Id }, createdTune);
		}
		private async Task<string> FileUpload(IFormFile file, string folderPath)
		{
			if (file == null || file.Length == 0)
				return string.Empty;

			string webRootPath = _web.WebRootPath;
			string uploadPath = Path.Combine(webRootPath, folderPath);

			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}

			string fileName = Path.GetFileName(file.FileName);
			string filePath = Path.Combine(uploadPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return Path.Combine(folderPath, fileName).Replace('\\', '/');
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
