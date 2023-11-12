using System;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.Flags
{
    public enum ApiEndpoint : byte
    {
        USER = 0,
        GUILDS,
        GUILD_MEMBER,
        CONNECTIONS,
    }
}
