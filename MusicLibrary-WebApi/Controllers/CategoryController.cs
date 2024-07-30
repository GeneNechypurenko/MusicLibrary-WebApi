using Microsoft.AspNetCore.Mvc;
using MusicLibrary_WebApi.Models;
using MusicLibraryApp.BLL.ModelsDTO;
using MusicLibraryApp.BLL.Services.Interfaces;

namespace MusicLibrary_WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly IService<CategoryDTO> _categoryService;

		public CategoryController(IService<CategoryDTO> categoryService)
		{
			_categoryService = categoryService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCategories()
		{
			var categories = await _categoryService.GetAllAsync();
			return Ok(categories);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategory(int id)
		{
			var category = await _categoryService.GetAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			return Ok(category);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromBody] CategoryModel model)
		{
			if (model == null)
			{
				return BadRequest("Model is null");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var categoryDTO = new CategoryDTO
			{
				Genre = model.Genre
			};

			await _categoryService.CreateAsync(categoryDTO);

			return CreatedAtAction(nameof(GetCategory), new { id = categoryDTO.Id }, categoryDTO);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryModel model)
		{
			if (model == null || id <= 0)
			{
				return BadRequest("Invalid data.");
			}

			var existingCategory = await _categoryService.GetAsync(id);
			if (existingCategory == null)
			{
				return NotFound();
			}

			var categoryDTO = new CategoryDTO
			{
				Id = id,
				Genre = model.Genre
			};

			await _categoryService.UpdateAsync(categoryDTO);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var existingCategory = await _categoryService.GetAsync(id);
			if (existingCategory == null)
			{
				return NotFound();
			}

			await _categoryService.DeleteAsync(id);
			return NoContent();
		}
	}
}
