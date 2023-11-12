using System;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags
{
    /// <summary>
    /// https://discord.com/developers/docs/resources/user#user-object-premium-types
    /// </summary>
    [Flags]
    public enum PremiumFlags
    {
        /// <summary>
        /// No subscription.
        /// </summary>
        None = 0,

        /// <summary>
        /// Nitro Classic subscription.
        /// </summary>
        NitroClassic = 1,

        /// <summary>
        /// Nitro subscription.
        /// </summary>
        Nitro = 2,

        /// <summary>
        /// Nitro Basic subscription.
        /// </summary>
        NitroBasic = 3
    }
}
