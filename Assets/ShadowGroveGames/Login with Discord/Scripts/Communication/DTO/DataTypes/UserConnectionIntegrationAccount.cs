using Newtonsoft.Json;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.DataTypes
{
    public readonly struct UserConnectionIntegrationAccount
    {
        [JsonProperty("id")]
        public readonly string Id;

        [JsonProperty("name")]
        public readonly string Name;
    }
}