using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.UserGuilds
{
    public class GuildEntryScript : MonoBehaviour
    {
        [SerializeField]
        private RawImage _image;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _ownership;

        public void Show(UserGuild userGuild)
        {
            if (userGuild.IconUrl != null)
                StartCoroutine(GetImage(userGuild.IconUrl));

            _name.text = userGuild.Name;
            _ownership.text = $"<b>Ownership:</b> {userGuild.Owner}";

            _name.gameObject.SetActive(true);
            _ownership.gameObject.SetActive(true);
        }

        private IEnumerator GetImage(string guildImageUrl)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(guildImageUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture profileTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                _image.texture = profileTexture;
                _image.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
    }
}
