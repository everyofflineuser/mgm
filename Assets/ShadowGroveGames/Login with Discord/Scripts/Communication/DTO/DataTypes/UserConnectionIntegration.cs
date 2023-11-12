using Newtonsoft.Json;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags;
using System;

#nullable enable
namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.DataTypes
{
    public readonly struct UserConnectionIntegration
    {
        [JsonProperty("guild_id")]
        public readonly ulong? GuildId;

        [JsonProperty("id")]
        public readonly ulong Id;

        [JsonProperty("name")]
        public readonly string Name;

        [JsonProperty("type")]
        public readonly string Type;

        [JsonProperty("enabled")]
        public readonly bool Enabled;

        [JsonProperty("syncing")]
        public readonly bool? Syncing;

        [JsonProperty("role_id")]
        public readonly ulong? RoleId;

        [JsonProperty("enable_emoticons")]
        public readonly bool? EnableEmoticons;

        [JsonProperty("expire_behavior")]
        public readonly UserConnectionIntegrationExpireBehavior ExpireBehavior;

        [JsonProperty("expire_grace_period")]
        public readonly int? ExpireGracePeriod;

        [JsonProperty("user")]
        public readonly User? User;

        [JsonProperty("account")]
        public readonly UserConnectionIntegrationAccount? Account;

        [JsonProperty("synced_at")]
        public readonly DateTimeOffset? SyncedAt;

        [JsonProperty("subscriber_count")]
        public readonly int? SubscriberAccount;

        [JsonProperty("revoked")]
        public readonly bool? Revoked;

        [JsonProperty("application")]
        public readonly UserConnectionIntegrationApplication? Application;
    }
}
#nullable disable