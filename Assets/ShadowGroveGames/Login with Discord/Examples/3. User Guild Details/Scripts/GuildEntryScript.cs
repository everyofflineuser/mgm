using ShadowGroveGames.LoginWithDiscord.Scripts;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.UserGuildDetails
{
    public class GuildEntryScript : MonoBehaviour
    {
        [SerializeField]
        private RawImage _image;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _ownership;

        private ulong _guildId;

        private GuildMemberDetailViewScript _guildMemberDetailViewScript;

        public void Show(UserGuild userGuild, GuildMemberDetailViewScript guildMemberDetailViewScript)
        {
            if (userGuild.IconUrl != null)
                StartCoroutine(GetImage(userGuild.IconUrl));
            
            _guildId = userGuild.Id;
            _guildMemberDetailViewScript = guildMemberDetailViewScript;
            _name.text = userGuild.Name;
            _ownership.text = $"<b>Ownership:</b> {userGuild.Owner}";
        }

        public void OpenDetailView()
        {
            var guildUser = LoginWithDiscordScript.Instance.GetGuildUser(_guildId);
            if (guildUser == null)
                return;

            _guildMemberDetailViewScript.ShowGuildMember(guildUser.Value);
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
