using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.DataTypes;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Helper;
using System;
using System.Collections.Generic;

#nullable enable
namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/user#connection-object
    /// </summary>
    public readonly struct UserConnection
    {
        /// <summary>
        /// Id of the connection account
        /// </summary>
        [JsonProperty("id")]
        public readonly string Id;

        /// <summary>
        /// The username of the connection account
        /// </summary>
        [JsonProperty("name")]
        public readonly string Username;

        /// <summary>
        /// The service of this connection
        /// </summary>
        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
        public readonly IntegrationServiceType Type;

        /// <summary>
        /// Whether the connection is revoked
        /// </summary>
        [JsonProperty("revoked")]
        public readonly bool? Revoked;

        /// <summary>
        /// An list of partial server integrations
        /// </summary>
        [JsonProperty("integrations")]
        public readonly IReadOnlyCollection<UserConnectionIntegration>? Integrations;

        /// <summary>
        /// Whether the connection is verified
        /// </summary>
        [JsonProperty("verified")]
        public readonly bool Verified;

        /// <summary>
        /// Whether friend sync is enabled for this connection
        /// </summary>
        [JsonProperty("friend_sync")]
        public readonly bool FriendSync;

        /// <summary>
        /// Whether activities related to this connection will be shown in presence updates
        /// </summary>
        [JsonProperty("show_activity")]
        public readonly bool ShowActivity;

        /// <summary>
        /// Whether this connection has a corresponding third party OAuth2 token
        /// </summary>
        [JsonProperty("two_way_link")]
        public readonly bool TwoWayLink;

        /// <summary>
        /// Visibility of this connection
        /// </summary>
        [JsonProperty("visibility")]
        public readonly ConnectionVisibility Visibility;

        /// <summary>
        /// Metadata Visibility of this connection
        /// </summary>
        [JsonProperty("metadata_visibility")]
        public readonly MetadataVisibility MetadataVisibility;

        public readonly string Name => StringHelper.UppercaseFirst(Enum.GetName(typeof(IntegrationServiceType), Type).ToLower());
    }
}
#nullable disable