using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowGroveGames.LoginWithDiscord.Examples.LinkedThirdPartyAccounts
{
    public class ConnectionEntryScript : MonoBehaviour
    {
        [SerializeField]
        private Text _firstLetter;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _verified;

        [SerializeField]
        private Text _twoWayLink;

        [SerializeField]
        private Text _revoked;

        [SerializeField]
        private Text _showActivity;

        public void Show(UserConnection userConnection)
        {
            string connectionName = userConnection.Name;

            _firstLetter.text = connectionName[0].ToString();
            _name.text = $"{connectionName}: {userConnection.Username}";
            _verified.text = $"<b>Verified:</b> {userConnection.Verified}";
            _twoWayLink.text = $"<b>TwoWayLink:</b> {userConnection.TwoWayLink}";
            _revoked.text = $"<b>Revoked:</b> {userConnection.Revoked ?? false}";
            _showActivity.text = $"<b>ShowActivity:</b> {userConnection.ShowActivity}";
        }
    }
}
