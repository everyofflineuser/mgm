using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Scripts
{
    [Serializable]
    public class OAuthPermissions
    {
        public const string USER = "identify";
        public const string USER_WITH_EMAIL = "email";
        public const string GUILDS = "guilds";
        public const string GUILD_MEMBER = "guilds.members.read";
        public const string CONNECTIONS = "connections";

        /// <summary>
        /// Discord permission: identify
        /// </summary>
        [SerializeField]
        [Tooltip("Request access to user information")]
        public bool User = true;

        /// <summary>
        /// Discord permission: email
        /// </summary>
        [SerializeField]
        [Tooltip("Request access to the user's e-mail address")]
        public bool UserWithEmail = false;

        /// <summary>
        /// Discord permission: guilds
        /// </summary>
        [SerializeField]
        [Tooltip("Basic information about all guilds of the user")]
        public bool Guilds = false;

        /// <summary>
        /// Discord permission: guilds.members.read
        /// </summary>
        [SerializeField]
        [Tooltip("Request to access member information of a specific guild the user is in. Like ownership, roles, guild avatar, etc.")]
        public bool GuildMember = false;

        /// <summary>
        /// Discord permission: connections
        /// </summary>
        [SerializeField]
        [Tooltip("Request to access linked third-party accounts")]
        public bool Connections = false;

        public List<string> GetDiscordPermission()
        {
            var permissions = new HashSet<string>();

            if (UserWithEmail)
            {
                User = true;
                permissions.Add(USER_WITH_EMAIL);
            }

            if (User)
                permissions.Add(USER);

            if (GuildMember)
            {
                Guilds = true;
                permissions.Add(GUILD_MEMBER);
            }

            if (Guilds)
                permissions.Add(GUILDS);

            if (Connections)
                permissions.Add(CONNECTIONS);

            return permissions.ToList();
        }
    }
}
