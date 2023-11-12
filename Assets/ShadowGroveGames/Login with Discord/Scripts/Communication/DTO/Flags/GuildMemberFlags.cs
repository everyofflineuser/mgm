namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags
{
    public enum GuildMemberFlags
    {
        /// <summary>
        /// Member has no flags set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Member has left and rejoined the guild.
        /// </summary>
        /// <remarks>
        /// Cannot be modified.
        /// </remarks>
        DidRejoin = 1 << 0,

        /// <summary>
        /// Member has completed onboarding.
        /// </summary>
        /// <remarks>
        /// Cannot be modified.
        /// </remarks>
        CompletedOnboarding = 1 << 1,

        /// <summary>
        /// Member bypasses guild verification requirements.
        /// </summary>
        BypassesVerification = 1 << 2,

        /// <summary>
        /// Member has started onboarding.
        /// </summary>
        StartedOnboarding = 1 << 3,
    }
}
