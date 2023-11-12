using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Struct
{
    public struct OAuthToken
    {
        public string ApplicationId;
        public string AccessToken;
        public string TokenType;
        public int ExpiresIn;
        public DateTime UtcCreatedAt;
        public DateTime UtcExpiresAt;
        public List<string> Scope;

        [JsonIgnore]
        public string State;

        [JsonIgnore]
        public bool IsExpired => (UtcExpiresAt == null || UtcExpiresAt <= DateTime.UtcNow);
    }
}
