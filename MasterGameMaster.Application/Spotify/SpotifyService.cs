﻿using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MasterGameMaster.Application.Spotify
{
    public interface ISpotifyService
    {
        public Task<SpotifyTokens> GetSpotifyTokens(string code, string clientId, string secret, string spotifyBaseUri, string redirectUriBase);
        public Task<SpotifyTokens> RefreshToken(string refreshToken, string clientId, string secret, string spotifyBaseUri);
    }

    public class SpotifyService : ISpotifyService
    {

        public SpotifyService() { }

        public async Task<SpotifyTokens> GetSpotifyTokens(string code, string clientId, string secret, string spotifyBaseUri, string redirectUriBase)
        {
            RestClient client = PrepareSpotifyClient(clientId, secret, spotifyBaseUri);
            var request = new RestRequest("/token");
            string authorizationValue = GetSpotifyAuthorizationHeaderValue(clientId, secret);

            request.AddHeader("Authorization", authorizationValue);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", $"{redirectUriBase}/spotify-login");

            var response = await client.ExecutePostAsync<SpotifyTokensResponse>(request);

            if (response is not null && response.StatusCode is HttpStatusCode.OK && response.Data is not null)
            {
                var data = response.Data;
                return new SpotifyTokens(
                    data.AccessToken, data.RefreshToken, data.ExpiresIn);
            }
            else
                throw GetResponseException(response);
        }

        public async Task<SpotifyTokens> RefreshToken(string refreshToken, string clientId, string secret, string spotifyBaseUri)
        {
            var requestOptions = new RestClientOptions(spotifyBaseUri)
            {
                Authenticator = new HttpBasicAuthenticator(clientId, secret)
            };
            var client = new RestClient(requestOptions, configureSerialization: s => s.UseSystemTextJson(
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                }));
            var request = new RestRequest("/token");

            var authorizationValue = GetSpotifyAuthorizationHeaderValue(clientId, secret);

            request.AddHeader("Authorization", authorizationValue);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", refreshToken);

            var response = await client.ExecutePostAsync<SpotifyTokensResponse>(request);

            if (response is not null && response.StatusCode is HttpStatusCode.OK && response.Data is not null)
            {
                var data = response.Data;
                return new SpotifyTokens(
                    data.AccessToken, data.RefreshToken, data.ExpiresIn);
            }
            else
                throw GetResponseException(response);
        }

        private static RestClient PrepareSpotifyClient(string clientId, string secret, string spotifyBaseUri)
        {
            var requestOptions = new RestClientOptions(spotifyBaseUri)
            {
                Authenticator = new HttpBasicAuthenticator(clientId, secret)
            };
            var client = new RestClient(requestOptions, configureSerialization: s => s.UseSystemTextJson(
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                }));
            return client;
        }

        private static string GetSpotifyAuthorizationHeaderValue(string clientId, string secret)
        {
            var authorizationBytes = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
            var authorizationBase = Convert.ToBase64String(authorizationBytes);
            var authorizationValue = $"Basic {authorizationBase}:";
            return authorizationValue;
        }

        private static Exception GetResponseException(RestResponse<SpotifyTokensResponse>? response)
        {
            Exception exception;

            if (response is null)
                exception = new SpotifyTokenException("Error while fetching tokens, response is null");
            else if (response.StatusCode is not HttpStatusCode.OK)
                exception = new SpotifyTokenException($"Error while fetching tokens, status code: {response.StatusCode}");
            else
                exception = new SpotifyTokenException("Unknown error occured");

            return exception;
        }
    }
}
