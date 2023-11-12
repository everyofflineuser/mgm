using Newtonsoft.Json;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.Exceptions;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.Flags;
using ShadowGroveGames.LoginWithDiscord.Scripts.Struct;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#nullable enable
namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication
{
    public class LoginWithDiscordClient
    {
        private const string BASE_URL = "https://discord.com/api/v10";
        private const string HEADER_RATE_LIMIT_REMAINING = "x-ratelimit-remaining";
        private const string HEADER_RATE_LIMIT_LIMIT = "x-ratelimit-limit";

        private OAuthToken _oAuthToken;

        /// <summary>
        /// Events
        /// </summary>
        public event RateLimitEvent? OnRateLimitWarningEvent;
        public event RateLimitEvent? OnRateLimitReachedEvent;

        public LoginWithDiscordClient(OAuthToken oAuthToken)
        {
            _oAuthToken = oAuthToken;
        }

        public User? GetUser()
        {
            if (!_oAuthToken.Scope.Contains(OAuthPermissions.USER))
                throw new InsufficientPermissionsException("You do not have access to user information. Request this permission before calling this method.");

            var responseBody = SendGetRequest("/users/@me", ApiEndpoint.USER);
            if (responseBody == null)
                return null;

            return JsonConvert.DeserializeObject<User>(responseBody);
        }

        public List<UserGuild>? GetUserGuilds()
        {
            if (!_oAuthToken.Scope.Contains(OAuthPermissions.GUILDS))
                throw new InsufficientPermissionsException("You do not have access to guilds information. Request this permission before calling this method.");

            var responseBody = SendGetRequest("/users/@me/guilds", ApiEndpoint.GUILDS);
            if (responseBody == null)
                return null;

            return JsonConvert.DeserializeObject<List<UserGuild>>(responseBody);
        }

        public GuildMember? GetGuildMember(ulong guildId)
        {
            if (!_oAuthToken.Scope.Contains(OAuthPermissions.GUILD_MEMBER))
                throw new InsufficientPermissionsException("You do not have access to member information of a specific guild the user is in. Request this permission before calling this method.");

            var responseBody = SendGetRequest($"/users/@me/guilds/{guildId}/member", ApiEndpoint.GUILD_MEMBER);
            if (responseBody == null)
                return null;

            var guildMember = JsonConvert.DeserializeObject<GuildMember>(responseBody);
            guildMember.GuildId = guildId;

            return guildMember;
        }

        public List<UserConnection>? GetUserConnections()
        {
            if (!_oAuthToken.Scope.Contains(OAuthPermissions.CONNECTIONS))
                throw new InsufficientPermissionsException("You do not have access to linked third-party accounts. Request this permission before calling this method.");

            var responseBody = SendGetRequest("/users/@me/connections", ApiEndpoint.CONNECTIONS);
            if (responseBody == null)
                return null;

            return JsonConvert.DeserializeObject<List<UserConnection>>(responseBody);
        }

        private string? SendGetRequest(string url, ApiEndpoint scope)
        {
            if (_oAuthToken.IsExpired)
            {
                Debug.LogError("Discord OAuth token is expired!");
                return null;
            }

            url = url.Trim().Trim('/');

            try
            {
                using (UnityWebRequest webRequest = UnityWebRequest.Get($"{BASE_URL}/{url}"))
                {
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                    webRequest.SetRequestHeader("Authorization", $"{_oAuthToken.TokenType} {_oAuthToken.AccessToken}");
                    webRequest.timeout = 5; // 5 second timeout

                    webRequest.SendWebRequest();
                    while (!webRequest.isDone) { }

                    HandleRateLimit(webRequest.GetResponseHeaders(), scope);

                    string? contentBody = null;

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                        case UnityWebRequest.Result.ProtocolError:
                            throw new Exception(webRequest.error);
                        case UnityWebRequest.Result.Success:
                            contentBody = webRequest.downloadHandler.text;
                            break;
                    }

                    if (contentBody == null)
                        throw new ArgumentNullException("Response from discord api is null!");

                    return contentBody;
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            return null;
        }

        private void HandleRateLimit(Dictionary<string, string> headers, ApiEndpoint endpoint)
        {
            var remaining = int.Parse(headers[HEADER_RATE_LIMIT_REMAINING] ?? "0");
            var limit = int.Parse(headers[HEADER_RATE_LIMIT_LIMIT] ?? "0");

            if (remaining == 0 && limit == 0)
                return;

            float rateLimitUsage = ((limit - remaining) / (float)limit);

            // Rate limit under 75%
            if (rateLimitUsage < 0.75f)
                return;

            // Rate limit over 75%
            if (rateLimitUsage < 1f)
            {
                OnRateLimitWarningEvent?.Invoke(remaining, limit, endpoint);
                return;
            }

            // Rate limit reched
            OnRateLimitReachedEvent?.Invoke(remaining, limit, endpoint);
        }
    }

    public delegate void RateLimitEvent(int remaining, int limit, ApiEndpoint endpoint);
}
#nullable disable