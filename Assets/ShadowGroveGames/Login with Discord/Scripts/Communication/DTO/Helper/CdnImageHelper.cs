using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Helper
{
    /// <summary>
    /// https://discord.com/developers/docs/reference#image-formatting
    /// </summary>
    public static class CdnImageHelper
    {
        public const string BASE_URL = "https://cdn.discordapp.com";

        /// <summary>
        /// Returns a user avatar URL.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="avatarId">The avatar identifier.</param>
        /// <param name="size">The size of the image to return in horizontal pixels. This can be any power of two between 16 and 2048.</param>
        /// <param name="allowAnimated">Allow animated user avatar.</param>
        /// <returns>
        /// A URL pointing to the user's avatar in the specified size.
        /// </returns>
        public static string GetUserAvatarUrl(ulong userId, string avatarId, ushort size = 512, bool allowAnimated = false)
        {
            if (avatarId == null)
                return null;

            string extension = GetExtension(avatarId, allowAnimated);
            return $"{BASE_URL}/avatars/{userId}/{avatarId}.{extension}?size={size}";
        }

        /// <summary>
        /// Returns a user avatar URL.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="guildId">The guild identifier.</param>
        /// <param name="avatarId">The avatar identifier.</param>
        /// <param name="size">The size of the image to return in horizontal pixels. This can be any power of two between 16 and 2048.</param>
        /// <param name="allowAnimated">Allow animated user avatar.</param>
        /// <returns>
        /// A URL pointing to the user's guild avatar in the specified size.
        /// </returns>
        public static string GetGuildUserAvatarUrl(ulong userId, ulong guildId, string avatarId, ushort size = 512, bool allowAnimated = false)
        {
            if (avatarId == null)
                return null;

            string extension = GetExtension(avatarId, allowAnimated);
            return $"{BASE_URL}/guilds/{guildId}/users/{userId}/avatars/{avatarId}.{extension}?size={size}";
        }

        /// <summary>
        /// Returns a user banner URL.
        /// </summary>
        /// <param name="userId">The user snowflake identifier.</param>
        /// <param name="bannerId">The banner identifier.</param>
        /// <param name="size">The size of the image to return in horizontal pixels. This can be any power of two between 16 and 2048.</param>
        /// <param name="allowAnimated">Allow animated user avatar.</param>
        /// <returns>
        /// A URL pointing to the user's banner in the specified size.
        /// </returns>
        public static string GetUserBannerUrl(ulong userId, string bannerId, ushort size = 480, bool allowAnimated = false)
        {
            if (bannerId == null)
                return null;

            string extension = GetExtension(bannerId, allowAnimated);
            return $"{BASE_URL}/banners/{userId}/{bannerId}.{extension}?size={size}";
        }


        /// <summary>
        /// Returns an icon URL.
        /// </summary>
        /// <param name="guildId">The guild snowflake identifier.</param>
        /// <param name="iconId">The icon identifier.</param>
        /// <param name="size">The size of the image to return in horizontal pixels. This can be any power of two between 16 and 2048.</param>
        /// <param name="allowAnimated">Allow animated user avatar.</param>
        /// <returns>
        /// A URL pointing to the guild's icon.
        /// </returns>
        public static string GetGuildIconUrl(ulong guildId, string iconId, ushort size = 512, bool allowAnimated = false)
        {
            if (iconId == null)
                return null;

            string extension = GetExtension(iconId, allowAnimated);
            return $"{BASE_URL}/icons/{guildId}/{iconId}.{extension}";
        }

        private static string GetExtension(string imageId, bool allowAnimated = false)
        {
            if (!allowAnimated)
                return "png";

            return imageId.StartsWith("a_") ? "gif" : "png";
        }
    }
}
