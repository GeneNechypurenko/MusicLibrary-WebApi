namespace MusicLibraryApp.BLL.ModelsDTO
{
	public class UserDTO
	{
		public int Id { get; set; }
		public string? Username { get; set; }
		public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
		public bool IsAuthorized { get; set; }
		public bool IsBlocked { get; set; }
	}
}
