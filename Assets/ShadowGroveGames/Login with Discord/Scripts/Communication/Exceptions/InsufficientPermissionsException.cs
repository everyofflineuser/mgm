using System;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication.Exceptions
{
    public class InsufficientPermissionsException : UnauthorizedAccessException
    {
        public InsufficientPermissionsException(string message) : base(message)
        {
        }
    }
}
