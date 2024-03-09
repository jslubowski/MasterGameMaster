using MasterGameMaster.Application.Spotify;
using Microsoft.AspNetCore.Mvc;

namespace MasterGameMaster.API.Controllers
{
    [Route("spotify")]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;
        
        private string _spotifyClientId;
        private string _spotifyClientSecret;
        private string _spotifyBaseUri;
        private string _frontendBaseUri;

        public SpotifyController(ISpotifyService spotifyService, IConfiguration configuration)
        {
            _spotifyService = spotifyService;
            _spotifyClientId = configuration.GetValue<string>("Spotify:ClientId");
            _spotifyClientSecret = configuration.GetValue<string>("Spotify:ClientSecret");
            _spotifyBaseUri = configuration.GetValue<string>("Spotify:BaseUrl");
            _frontendBaseUri = configuration.GetValue<string>("FrontendUrl");
        }

        [HttpGet("tokens")]
        public async Task<ActionResult<SpotifyTokens>> GetSpotifyTokens(string code)
        {
            var result = await _spotifyService.GetSpotifyTokens(code, _spotifyClientId, _spotifyClientSecret, _spotifyBaseUri, _frontendBaseUri);

            return Ok(result);
        }

        [HttpGet("refresh-tokens")]
        public async Task<ActionResult<SpotifyTokens>> RefreshSpotifyTokens(string refreshToken)
        {
            var result = await _spotifyService.RefreshToken(refreshToken, _spotifyClientId, _spotifyClientSecret, _spotifyBaseUri);

            return Ok(result);
        }
    }
}
