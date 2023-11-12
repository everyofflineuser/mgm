using ShadowGroveGames.LoginWithDiscord.Scripts;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.SimpleLogin
{
    public class LoggedInDetailViewScript : MonoBehaviour
    {
        [SerializeField]
        private Text _id;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _specialFlags;

        [SerializeField]
        private Text _bannerColor;

        [SerializeField]
        private Text _accentColor;

        [SerializeField]
        private Text _locale;

        [SerializeField]
        private Text _mfa;

        [SerializeField]
        private Text _nitro;

        [SerializeField]
        private Text _verified;

        [SerializeField]
        private Text _email;

        [SerializeField]
        private Text _badges;

        void OnEnable()
        {
            if (!LoginWithDiscordScript.Instance.IsReady)
                return;

            User? user = LoginWithDiscordScript.Instance.GetUser();
            if (user == null)
            {
                Debug.LogError("Cant fetch user from discord API!");
                return;
            }

            _id.text = $"ID: {user.Value.Id}";
            _name.text = $"<b>Username:</b> {user.Value.Username}#{user.Value.Discriminator}";
            _specialFlags.text = $"<b>Bot:</b> {user.Value.IsBotUser ?? false} | <b>System:</b> {user.Value.IsSystemUser ?? false}";
            _bannerColor.text = $"<b>BannerColor:</b> #{ColorUtility.ToHtmlStringRGB(user.Value.BannerColor ?? Color.black)}";
            _accentColor.text = $"<b>AccentColor:</b> #{ColorUtility.ToHtmlStringRGB(user.Value.AccentColor ?? Color.black)}";
            _locale.text = $"<b>Locale:</b> {user.Value.Locale ?? "Unknown"}";
            _mfa.text = $"<b>MFA:</b> {user.Value.MFA ?? false}";
            _nitro.text = $"<b>Nitro:</b> {Enum.GetName(typeof(PremiumFlags), user.Value.PremiumType ?? PremiumFlags.None)}";
            _verified.text = $"<b>Verified:</b> {user.Value.Verified ?? false}";
            _email.text = $"<b>E-Mail:</b> {user.Value.Email ?? "Hidden"}";
            _badges.text = $"<b>Badges:</b> {GetBadgeList(user)}";
        }

        private string GetBadgeList(User? user)
        {
            List<string> badgeList = new List<string>();
            if (user.Value.Flags.HasValue)
            {
                foreach (UserFlags userFlag in Enum.GetValues(typeof(UserFlags)))
                {
                    if (userFlag == UserFlags.None)
                        continue;

                    if (user.Value.Flags.Value.HasFlag(userFlag))
                        badgeList.Add(Enum.GetName(typeof(UserFlags), userFlag));
                }
            }

            return badgeList.Count > 0 ? string.Join(", ", badgeList) : "None";
        }

        public void CloseDetailView()
        {
            gameObject.SetActive(false);
        }
    }
}
