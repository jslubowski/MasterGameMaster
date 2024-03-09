namespace MasterGameMaster.Application.Spotify
{
    internal record SpotifyTokensResponse(
        string AccessToken, int ExpiresIn, string RefreshToken
        );
}
