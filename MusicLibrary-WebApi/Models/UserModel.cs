namespace MusicLibrary_WebApi.Models
{
	public class UserModel
	{
		public string Username { get; set; }
		public bool IsAuthorized { get; set; }
		public bool IsBlocked { get; set; }
	}
}
