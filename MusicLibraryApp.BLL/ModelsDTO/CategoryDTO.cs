using MusicLibraryApp.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicLibraryApp.BLL.ModelsDTO
{
	public class CategoryDTO
	{
		public int Id { get; set; }
		public string? Genre { get; set; }
		public IEnumerable<Tune>? Tunes { get; set; }
	}
}
