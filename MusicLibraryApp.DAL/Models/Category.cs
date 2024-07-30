namespace MusicLibraryApp.DAL.Models
{
	public class Category
	{
		public int Id { get; set; }
		public string? Genre { get; set; }
		public ICollection<Tune>? Tunes { get; set; }
	}
}
