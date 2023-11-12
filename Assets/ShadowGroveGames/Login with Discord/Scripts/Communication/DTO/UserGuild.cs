using Newtonsoft.Json;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Converter;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.DataTypes;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Helper;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/user#get-current-user-guilds
    /// </summary>
    public readonly struct UserGuild
    {
        /// <summary>
        /// Guild Id
        /// </summary>
        [JsonProperty("id")]
        public readonly ulong Id;

        /// <summary>
        /// Guild name (2-100 characters, excluding trailing and leading whitespace)
        /// </summary>
        [JsonProperty("name")]
        public readonly string Name;

        /// <summary>
        /// Guild icon hash
        /// </summary>
        [JsonProperty("icon")]
        public readonly string IconHash;

        /// <summary>
        /// True if the user is the owner of the guild
        /// </summary>
        [JsonProperty("owner")]
        public readonly bool Owner;

        /// <summary>
        /// Total permissions for the user in the guild (excludes overwrites)
        /// </summary>
        [JsonProperty("permissions"), JsonConverter(typeof(GuildPermissionsConverter))]
        public readonly GuildPermissions Permissions;

        /// <summary>
        /// Enabled guild features
        /// </summary>
        [JsonProperty("features"), JsonConverter(typeof(GuildFeaturesConverter))]
        public readonly GuildFeatures Features;

        public string IconUrl => CdnImageHelper.GetGuildIconUrl(Id, IconHash);

        public string UserAvatarUrl(ulong userId) => CdnImageHelper.GetGuildUserAvatarUrl(userId, Id, IconHash);
    }
}
