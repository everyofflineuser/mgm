using ShadowGroveGames.LoginWithDiscord.Examples.LinkedThirdPartyAccounts;
using ShadowGroveGames.LoginWithDiscord.Scripts;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.Flags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UserGuildsDTO = ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO.UserGuild;

namespace ShadowGroveGames.LoginWithDiscord.Examples.UserGuildDetails
{
    public class UserGuildsViewScript : MonoBehaviour
    {
        [SerializeField]
        private RawImage _profileImage;

        [SerializeField]
        private Text _name;

        [Space]

        [SerializeField]
        private GameObject _noGuildsMessage;

        [SerializeField]
        private Transform _guildsContainer;

        [SerializeField]
        private GuildEntryScript _guildsPrefab;

        [Space]
        [SerializeField]
        private GuildMemberDetailViewScript _guildMemberDetailViewScript;

        public void OnLoginSuccess()
        {
            gameObject.SetActive(true);

            User? user = LoginWithDiscordScript.Instance.GetUser();
            if (user != null)
            {
                _name.text = $"{user.Value.Username}#{user.Value.Discriminator}";

                if (user.Value.AvatarUrl != null)
                    StartCoroutine(GetProfileImage(user.Value.AvatarUrl));
            }

            ShowUserGuilds();
        }

        public void OnLoginFailure()
        {
            gameObject.SetActive(false);
        }

        public void OnRateLimitWarningEvent(int remaining, int limit, ApiEndpoint endpoint)
        {
            if (endpoint != ApiEndpoint.GUILD_MEMBER)
                return;

            Debug.LogWarning($"RateLimit Warning: You used more then 75% ({limit - remaining} / {limit}) of the rate limit for the \"{System.Enum.GetName(typeof(ApiEndpoint), endpoint)}\" endpoint.");
        }

        public void OnRateLimitReachedEvent(int remaining, int limit, ApiEndpoint endpoint)
        {
            Debug.LogWarning($"RateLimit Reached: You used more then 100% ({limit-remaining} / {limit}) of the rate limit for the \"{System.Enum.GetName(typeof(ApiEndpoint), endpoint)}\" endpoint!");
        }

        private IEnumerator GetProfileImage(string profileImageUrl)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(profileImageUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture profileTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                _profileImage.texture = profileTexture;
            }
            else
            {
                Debug.LogError(www.error);
            }
        }

        private void ShowUserGuilds()
        {
            List<UserGuildsDTO> userGuilds = LoginWithDiscordScript.Instance.GetUserGuilds();
            if (userGuilds == null)
            {
                Debug.LogError("Cant fetch user guilds from discord API!");
                return;
            }

            if (userGuilds.Count == 0)
            {
                _noGuildsMessage.SetActive(true);
                return;
            }

            foreach (UserGuildsDTO userGuild in userGuilds)
            {
                GuildEntryScript guildEntry = Instantiate(_guildsPrefab, _guildsContainer, false);
                guildEntry.Show(userGuild, _guildMemberDetailViewScript);
            }
        }
    }
}