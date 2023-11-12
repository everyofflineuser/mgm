namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags
{
    public enum UserConnectionIntegrationExpireBehavior
    {
        /// <summary>
        /// Removes a role from an expired subscriber.
        /// </summary>
        RemoveRole = 0,

        /// <summary>
        /// Kicks an expired subscriber from the guild.
        /// </summary>
        Kick = 1
    }
}
