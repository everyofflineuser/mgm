using Newtonsoft.Json;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Helper;
using System;
using System.Collections.Generic;

#nullable enable
namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/guild#guild-member-object
    /// </summary>
    public struct GuildMember
    {
        /// <summary>
        /// The id of the guild
        /// </summary>
        public ulong GuildId;

        /// <summary>
        /// The user this guild member represents
        /// </summary>
        [JsonProperty("user")]
        public readonly User? User;

        /// <summary>
        /// This user's guild nickname
        /// </summary>
        [JsonProperty("nick")]
        public readonly string? Nick;

        /// <summary>
        /// The member's guild avatar hash
        /// </summary>
        [JsonProperty("avatar")]
        public readonly string? AvatarHash;

        /// <summary>
        /// List of role ids
        /// </summary>
        [JsonProperty("roles")]
        public readonly IReadOnlyCollection<ulong> Roles;

        /// <summary>
        /// When the user joined the guild
        /// </summary>
        [JsonProperty("joined_at")]
        public readonly DateTimeOffset JoinedAt;

        /// <summary>
        /// When the user started boosting the guild
        /// </summary>
        [JsonProperty("premium_since")]
        public readonly DateTimeOffset? PremiumSince;

        /// <summary>
        /// Whether the user is deafened in voice channels
        /// </summary>
        [JsonProperty("deafened")]
        public readonly bool DeafenedInVoice;

        /// <summary>
        /// Whether the user is muted in voice channels
        /// </summary>
        [JsonProperty("mute")]
        public readonly bool MutedInVoice;

        /// <summary>
        /// Guild member flags
        /// </summary>
        [JsonProperty("flags")]
        public readonly GuildMemberFlags Flags;

        /// <summary>
        /// Whether the user has not yet passed the guild's Membership Screening requirements
        /// </summary>
        [JsonProperty("pending")]
        public readonly bool? Pending;

        /// <summary>
        /// When the user's timeout will expire and the user will be able to communicate in the guild again, null or a time in the past if the user is not timed out
        /// </summary>
        [JsonProperty("communication_disabled_until")]
        public readonly DateTimeOffset? TimeoutUntil;

        public string? UserAvatarUrl => User.HasValue ? CdnImageHelper.GetGuildUserAvatarUrl(User.Value.Id, GuildId, AvatarHash) : null; 
    }
}
#nullable disable