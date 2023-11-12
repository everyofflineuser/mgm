using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.Flags;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.UserGuildDetails
{
    public class GuildMemberDetailViewScript : MonoBehaviour
    {
        [SerializeField]
        private RawImage _guildProfileImage;

        [SerializeField]
        private Text _name;

        [Space()]

        [SerializeField]
        private Text _guildId;

        [SerializeField]
        private Text _nick;

        [SerializeField]
        private Text _joinedAt;

        [SerializeField]
        private Text _premiumSince;

        [SerializeField]
        private Text _deafenedInVoice;

        [SerializeField]
        private Text _mutedInVoice;

        [SerializeField]
        private Text _flags;

        [SerializeField]
        private Text _pending;

        [SerializeField]
        private Text _timeoutUntil;

        [SerializeField]
        private Text _roles;

        public void ShowGuildMember(GuildMember guildUser)
        {
            string guildNickname = guildUser.Nick ?? guildUser.User?.Username ?? "NoAccess";
            string guildProfileImage = guildUser.UserAvatarUrl ?? guildUser.User?.AvatarUrl;

            _name.text = $"{guildNickname}#{guildUser.User?.Discriminator ?? "0000"}";
            _guildId.text = $"<b>Guild ID:</b> {guildUser.GuildId}";
            _nick.text = $"<b>Nick:</b> {guildNickname}";
            _joinedAt.text = $"<b>JoinedAt:</b> {guildUser.JoinedAt.ToString("R")}"; // <b>JoinedAt:</b> Sat, 07 Jan 2023 14:01:13 GMT
            _premiumSince.text = $"<b>PremiumSince:</b> {guildUser.PremiumSince?.ToString("R") ?? "No boost"}";
            _deafenedInVoice.text = $"<b>DeafenedInVoice:</b> {guildUser.DeafenedInVoice}";
            _mutedInVoice.text = $"<b>MutedInVoice:</b> {guildUser.MutedInVoice}";
            _flags.text = $"<b>Flags:</b> {Enum.GetName(typeof(GuildMemberFlags), guildUser.Flags)}";
            _pending.text = $"<b>Pending:</b> {guildUser.Pending}";
            _timeoutUntil.text = $"<b>TimeoutUntil:</b> {guildUser.TimeoutUntil?.ToString("R") ?? "No timeout"}";
            _roles.text = $"<b>RoleIds:</b> {string.Join(", ", guildUser.Roles)}";

            gameObject.SetActive(true);
            if (guildProfileImage != null)
                StartCoroutine(GetProfileImage(guildProfileImage));
        }

        public void CloseDetailView()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator GetProfileImage(string profileImageUrl)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(profileImageUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture profileTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                _guildProfileImage.texture = profileTexture;
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
    }
}