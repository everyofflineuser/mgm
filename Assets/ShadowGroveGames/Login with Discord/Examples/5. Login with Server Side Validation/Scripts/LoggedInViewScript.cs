using Newtonsoft.Json;
using ShadowGroveGames.LoginWithDiscord.Scripts;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using ShadowGroveGames.LoginWithDiscord.Scripts.Struct;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.LoginWithServerSideValidation
{
    public class LoggedInViewScript : MonoBehaviour
    {
        [Header("Example Server Settings")]
        [SerializeField]
        [Tooltip("You example server url")]
        private string _serverUrl = "http://127.0.0.1:17791/";

        [Header("View Settings")]
        [SerializeField]
        private RawImage _banner;

        [SerializeField]
        private RawImage _profileImage;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _id;

        [SerializeField]
        private Text _welcomeMessage;

        [SerializeField]
        private GameObject _validateText;

        public void OnLoginSuccess()
        {
            gameObject.SetActive(true);

            OAuthToken? oAuthToken = LoginWithDiscordScript.Instance.GetOAuthToken();
            if (oAuthToken == null)
                return;

            StartCoroutine(GetInformationFromExampleServer((OAuthToken)oAuthToken, (UnityWebRequest request) =>
            {
                ServerSideUser? serverSideUser = JsonConvert.DeserializeObject<ServerSideUser>(request.downloadHandler.text);
                if (serverSideUser == null)
                {
                    Debug.LogError("Cant fetch user from server API!");
                    return;
                }

                _name.text = $"{serverSideUser.Value.Username}#{serverSideUser.Value.Discriminator}";
                _id.text = $"ID: {serverSideUser.Value.Id}";
                _welcomeMessage.text = serverSideUser.Value.WelcomeMessage;

                if (serverSideUser.Value.AvatarUrl != null)
                {
                    StartCoroutine(GetProfileImage(serverSideUser.Value.AvatarUrl));
                }

                if (serverSideUser.Value.BannerUrl != null)
                {
                    _banner.color = Color.white;
                    StartCoroutine(GetBannerImage(serverSideUser.Value.BannerUrl));
                }
                else if (serverSideUser.Value.AccentColor != null)
                {
                    _banner.color = serverSideUser.Value.AccentColor.Value;
                }

                _validateText.SetActive(false);
                _banner.gameObject.SetActive(true);
                _profileImage.gameObject.SetActive(true);
                _name.gameObject.SetActive(true);
                _id.gameObject.SetActive(true);
                _welcomeMessage.gameObject.SetActive(true);
            }));
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

        private IEnumerator GetBannerImage(string profileImageUrl)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(profileImageUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture bannerTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                _banner.texture = bannerTexture;
            }
            else
            {
                Debug.LogError(www.error);
            }
        }

        private IEnumerator GetInformationFromExampleServer(OAuthToken oAuthToken, Action<UnityWebRequest> successCallback)
        {
            using (UnityWebRequest www = new UnityWebRequest(_serverUrl, "POST"))
            {
                www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(oAuthToken)));
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    successCallback(www);
                }
                else
                {
                    Debug.LogError(www.error);
                }
            }
        }
    }
}
