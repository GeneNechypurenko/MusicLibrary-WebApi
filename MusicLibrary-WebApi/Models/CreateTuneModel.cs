namespace MusicLibrary_WebApi.Models
{
	public class CreateTuneModel
	{
		public string Performer { get; set; }
		public string Title { get; set; }
		public int CategoryId { get; set; }
		public bool IsAuthorized { get; set; }
		public bool IsBlocked { get; set; }

		public IFormFile File { get; set; }
		public IFormFile Poster { get; set; }
	}
}
