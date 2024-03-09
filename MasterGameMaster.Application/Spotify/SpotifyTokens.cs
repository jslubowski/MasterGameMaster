namespace MasterGameMaster.Application.Spotify
{
    public record SpotifyTokens(string AccessToken, string RefreshToken, int ExpiresIn);
}
