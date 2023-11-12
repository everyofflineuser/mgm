using ShadowGroveGames.LoginWithDiscord.Examples.LinkedThirdPartyAccounts;
using ShadowGroveGames.LoginWithDiscord.Scripts;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.UserGuilds
{
    public class UserGuildsViewScript : MonoBehaviour
    {
        [SerializeField]
        private RawImage _profileImage;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private GameObject _noGuildsMessage;

        [SerializeField]
        private Transform _guildsContainer;

        [SerializeField]
        private GuildEntryScript _guildsPrefab;

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
            List<UserGuild> userGuilds = LoginWithDiscordScript.Instance.GetUserGuilds();
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

            foreach (UserGuild userGuild in userGuilds)
            {
                GuildEntryScript guildEntry = Instantiate(_guildsPrefab, _guildsContainer, false);
                guildEntry.Show(userGuild);
            }
        }
    }
}