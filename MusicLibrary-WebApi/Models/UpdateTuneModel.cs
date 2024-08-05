namespace MusicLibrary_WebApi.Models
{
	public class UpdateTuneModel
	{
		public string Performer { get; set; }
		public string Title { get; set; }
		public int CategoryId { get; set; }
		public int IsAuthorize { get; set; }
		public int IsBlocked { get; set; }
		public IFormFile File { get; set; }
		public IFormFile Poster { get; set; }
	}
}
