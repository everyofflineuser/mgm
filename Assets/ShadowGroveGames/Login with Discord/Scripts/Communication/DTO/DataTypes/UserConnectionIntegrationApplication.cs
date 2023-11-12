using Newtonsoft.Json;

#nullable enable
namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.DataTypes
{
    public readonly struct UserConnectionIntegrationApplication
    {
        [JsonProperty("id")]
        public readonly ulong Id;

        [JsonProperty("name")]
        public readonly string Name;

        [JsonProperty("icon")]
        public readonly string? Icon;

        [JsonProperty("description")]
        public readonly string Description;

        [JsonProperty("summary")]
        public readonly string Summary;

        [JsonProperty("bot")]
        public readonly User? Bot;
    }
}
#nullable disable