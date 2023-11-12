using ShadowGroveGames.LoginWithDiscord.Scripts;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.LinkedThirdPartyAccounts
{
    public class UserConnectionViewScript : MonoBehaviour
    {
        public RawImage _profileImage;

        public Text _name;

        public GameObject _noLinkedConnectionMessage;

        public Transform _connectionContainer;

        public ConnectionEntryScript _connectionPrefab;

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

            ShowUserConnections();
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

        private void ShowUserConnections()
        {
            List<UserConnection> userConnections = LoginWithDiscordScript.Instance.GetUserConnections();

            if (userConnections.Count == 0)
            {
                _noLinkedConnectionMessage.SetActive(true);
                return;
            }

            List<ulong> allowedGuilds = new List<ulong>()
            {
                1047073186891698227L, // SGG
                489222168727519232L, // Unity
                681868752681304066L, // Manasoup
                280521930371760138L, // Game Dev Network
                330144558375501825L, // Pusheen
                830900174553481236L, // United Programming
                401433558813507584L, // Unity Dev Community
                656532291601825815L, // REWDAVID
                201544496654057472L, // Code Monkeys
            };

            foreach (UserConnection userConnection in userConnections)
            {
                //if (!allowedGuilds.Contains(userConnection.Id))
                //    continue;

                ConnectionEntryScript guildEntry = Instantiate(_connectionPrefab, _connectionContainer, false);
                guildEntry.Show(userConnection);
            }
        }
    }
}