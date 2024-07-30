namespace MusicLibrary_WebApi.Models
{
	public class TuneModel
	{
		public string Performer { get; set; }
		public string Title { get; set; }
		public bool IsAuthorized { get; set; }
		public bool IsBlocked { get; set; }
		public int CategoryId { get; set; }
	}
}
