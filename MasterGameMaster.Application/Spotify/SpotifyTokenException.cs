namespace MasterGameMaster.Application.Spotify
{
    internal class SpotifyTokenException: Exception
    {
        public SpotifyTokenException(string message) : base(message) { }
    }
}
