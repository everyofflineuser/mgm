using ShadowGroveGames.LoginWithDiscord.Scripts;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.SimpleLogin
{
    public class LoggedInViewScript : MonoBehaviour
    {
        [SerializeField]
        private RawImage _banner;

        [SerializeField]
        private RawImage _profileImage;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _id;

        [SerializeField]
        private GameObject _detailView;

        public void OnLoginSuccess()
        {
            gameObject.SetActive(true);

            User? user = LoginWithDiscordScript.Instance.GetUser();
            if (user == null)
            {
                Debug.LogError("Cant fetch user from discord API!");
                return;
            }

            _name.text = $"{user.Value.Username}#{user.Value.Discriminator}";
            _id.text = $"ID: {user.Value.Id}";

            if (user.Value.AvatarUrl != null)
            {
                StartCoroutine(GetProfileImage(user.Value.AvatarUrl));
            }

            if (user.Value.BannerUrl != null)
            {
                _banner.color = Color.white;
                StartCoroutine(GetBannerImage(user.Value.BannerUrl));
            }
            else if (user.Value.AccentColor != null)
            {
                _banner.color = user.Value.AccentColor.Value;
            }
        }

        public void OnLoginFailure()
        {
            gameObject.SetActive(false);
            _detailView.SetActive(false);
        }

        public void OpenDetailView()
        {
            _detailView.SetActive(true);
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
    }
}
