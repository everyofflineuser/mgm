using Newtonsoft.Json;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Converter;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Helper;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Examples.LoginWithServerSideValidation
{
#nullable enable
    public readonly struct ServerSideUser
    {
        /// <summary>
        /// The user's id
        /// </summary>
        [JsonProperty("id")]
        public readonly ulong Id;

        /// <summary>
        /// The user's username, not unique across the platform
        /// </summary>
        [JsonProperty("username")]
        public readonly string Username;

        /// <summary>
        /// The user's 4-digit discord-tag
        /// </summary>
        [JsonProperty("discriminator")]
        public readonly string Discriminator;

        /// <summary>
        /// The user's avatar hash
        /// </summary>
        [JsonProperty("avatar")]
        public readonly string AvatarHash;

        /// <summary>
        /// Whether the user belongs to an OAuth2 application
        /// </summary>
        [JsonProperty("bot")]
        public readonly bool? IsBotUser;

        /// <summary>
        /// Whether the user is an Official Discord System user (part of the urgent message system)
        /// </summary>
        [JsonProperty("system")]
        public readonly bool? IsSystemUser;

        /// <summary>
        /// The user's avatar decoration hash
        /// </summary>
        [JsonProperty("avatar_decoration")]
        public readonly string? AvatarDecorationHash;

        /// <summary>
        /// The public flags on a user's account
        /// </summary>
        [JsonProperty("public_flags")]
        public readonly UserFlags? PublicFlags;

        /// <summary>
        /// The flags on a user's account
        /// </summary>
        [JsonProperty("flags")]
        public readonly UserFlags? Flags;

        /// <summary>
        /// The user's banner hash
        /// </summary>
        [JsonProperty("banner")]
        public readonly string? BannerHash;

        /// <summary>
        /// The user's banner color
        /// </summary>
        [JsonProperty("banner_color"), JsonConverter(typeof(HexToColorConverter))]
        public readonly Color? BannerColor;

        /// <summary>
        /// The user's banner color
        /// </summary>
        [JsonProperty("accent_color"), JsonConverter(typeof(IntegerToColorConverter))]
        public readonly Color? AccentColor;

        /// <summary>
        /// The user's chosen language option
        /// </summary>
        [JsonProperty("locale")]
        public readonly string? Locale;

        /// <summary>
        /// Whether the user has two factor enabled on their account
        /// </summary>
        [JsonProperty("mfa_enabled")]
        public readonly bool? MFA;

        /// <summary>
        /// The type of Nitro subscription on a user's account
        /// </summary>
        [JsonProperty("premium_type")]
        public readonly PremiumFlags? PremiumType;

        /// <summary>
        /// Whether the email on this account has been verified
        /// </summary>
        [JsonProperty("verified")]
        public readonly bool? Verified;

        /// <summary>
        /// The user's email
        /// </summary>
        [JsonProperty("email")]
        public readonly string? Email;

        /// <summary>
        /// Server side welcome message
        /// </summary>
        [JsonProperty("welcomeMessage")]
        public readonly string WelcomeMessage;

        /// <summary>
        /// Get user's avatar url
        /// </summary>
        public string AvatarUrl => CdnImageHelper.GetUserAvatarUrl(Id, AvatarHash);

        /// <summary>
        /// Get user's bannar url
        /// </summary>
        public string BannerUrl => CdnImageHelper.GetUserBannerUrl(Id, BannerHash, 480, true);
    }
#nullable disable
}
