using ShadowGroveGames.LoginWithDiscord.Scripts.Communication;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.DTO;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.Exceptions;
using ShadowGroveGames.LoginWithDiscord.Scripts.Communication.Flags;
using ShadowGroveGames.LoginWithDiscord.Scripts.Struct;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace ShadowGroveGames.LoginWithDiscord.Scripts
{
    public class LoginWithDiscordScript : MonoBehaviour
    {
        private const string DISCORD_AUTH_BASE_URL = "https://discord.com/oauth2/authorize";

        [Header("Discord Settings")]
        [SerializeField]
        private string _applicationId;

        [SerializeField]
        private OAuthPermissions _requestPermissions;

        [Header("Callback Settings")]
        [SerializeField]
        [Tooltip("A list of ports which can be used for discord callbacks")]
        private List<int> _callbackPorts = new List<int>();

        [SerializeField]
        [Tooltip("Log Level for the callback server")]
        private ListenerLogLevel _logLevel = ListenerLogLevel.Error;

        [Header("Events")]
        [SerializeField]
        private OnLoginWithDiscordEvent _onLoginWithDiscordSuccess = new OnLoginWithDiscordEvent();

        [SerializeField]
        private OnLoginWithDiscordEvent _onLoginWithDiscordFailure = new OnLoginWithDiscordEvent();

        [SerializeField]
        private OnRateLimitEvent _onRateLimitWarningEvent = new OnRateLimitEvent();

        [SerializeField]
        private OnRateLimitEvent _onRateLimitRechedEvent = new OnRateLimitEvent();

        private OAuthToken? _oAuthToken = null;
        private string _serverStateKey;
        private LoginWithDiscordServer _loginWithDiscordServer;
        private LoginWithDiscordClient _loginWithDiscordClient;

        public static LoginWithDiscordScript Instance { get; private set; }

        public bool IsReady { get; private set; } = false;

        private void OnEnable()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);

            _serverStateKey = GenerateRandomStateKey();

            _loginWithDiscordServer = new LoginWithDiscordServer(new HashSet<int>(_callbackPorts), _logLevel, _serverStateKey, _applicationId);
            _loginWithDiscordServer.OnSuccess += LoginWithDiscordSuccess;
            _loginWithDiscordServer.OnFailure += LoginWithDiscordFailure;

            if (!_loginWithDiscordServer.Start())
            {
                Debug.Log("Cant start callback Server. Please check the logs!");
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (_loginWithDiscordServer == null)
                return;

            _loginWithDiscordServer.OnSuccess -= LoginWithDiscordSuccess;
            _loginWithDiscordServer.OnFailure -= LoginWithDiscordFailure;
            _loginWithDiscordServer.Stop();
        }

        private void OnDestroy()
        {
            OnDisable();
        }

        /// <summary>
        /// In order to process the requests that have been accepted in the Async mode, we need to process them here one by one
        /// </summary>
        private void FixedUpdate()
        {
            _loginWithDiscordServer.Process();
        }

        private void LoginWithDiscordSuccess(OAuthToken oAuthToken)
        {
            _oAuthToken = oAuthToken;
            _loginWithDiscordClient = new LoginWithDiscordClient(oAuthToken);
            _loginWithDiscordClient.OnRateLimitWarningEvent += RateLimitWarningEvent;
            _loginWithDiscordClient.OnRateLimitReachedEvent += RateLimitReachedEvent;

            IsReady = true;
            _onLoginWithDiscordSuccess?.Invoke();
        }

        private void LoginWithDiscordFailure()
        {
            IsReady = false;
            _oAuthToken = null;
            _onLoginWithDiscordFailure?.Invoke();
        }

        private void RateLimitWarningEvent(int remaining, int limit, ApiEndpoint endpoint)
        {
            _onRateLimitWarningEvent?.Invoke(remaining, limit, endpoint);
        }

        private void RateLimitReachedEvent(int remaining, int limit, ApiEndpoint endpoint)
        {
            _onRateLimitRechedEvent?.Invoke(remaining, limit, endpoint);
        }

        private string GenerateRandomStateKey(int length = 10)
        {
            // creating a StringBuilder object()
            StringBuilder stringBuilder = new StringBuilder();
            System.Random random = new System.Random();

            char letter;
            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);

                stringBuilder.Append(letter);
            }

            return stringBuilder.ToString();
        }

        private bool ValidateRequest()
        {
            if (!IsReady || _loginWithDiscordClient == null)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogError("The client is not ready, let the user authenticate before using this function!");

                return false;
            }

            return true;
        }

        public string GenerateDiscordOAuthUrl()
        {
            var permissions = _requestPermissions.GetDiscordPermission();
            if (permissions.Count == 0)
            {
                if (_logLevel >= ListenerLogLevel.Warning)
                    Debug.LogWarning("No permissions selected. Fallback to default permissions");

                permissions.Add(OAuthPermissions.USER);
            }

            return $"{DISCORD_AUTH_BASE_URL}?response_type=token&client_id={_applicationId}&scope={string.Join("%20", permissions)}&redirectUrl=127.0.0.1:{_loginWithDiscordServer.ActivePort}&state={_serverStateKey}";
        }

        public User? GetUser()
        {
            if (!ValidateRequest())
                return null;

            try
            {
                return _loginWithDiscordClient.GetUser();
            }
            catch (InsufficientPermissionsException ex)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogError(ex.Message);
            }

            return null;
        }

        public List<UserGuild> GetUserGuilds()
        {
            List<UserGuild> empty = new List<UserGuild>();

            if (!ValidateRequest())
                return empty;

            try
            {
                return _loginWithDiscordClient.GetUserGuilds() ?? empty;
            }
            catch (InsufficientPermissionsException ex)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogError(ex.Message);
            }

            return empty;
        }

        public GuildMember? GetGuildUser(ulong guildId)
        {
            if (!ValidateRequest())
                return null;

            try
            {
                return _loginWithDiscordClient.GetGuildMember(guildId);
            }
            catch (InsufficientPermissionsException ex)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogError(ex.Message);
            }

            return null;
        }

        public List<UserConnection> GetUserConnections()
        {
            List<UserConnection> empty = new List<UserConnection>();

            if (!ValidateRequest())
                return empty;

            try
            {
                return _loginWithDiscordClient.GetUserConnections() ?? empty;
            }
            catch (InsufficientPermissionsException ex)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogError(ex.Message);
            }

            return empty;
        }

        public OAuthToken? GetOAuthToken()
        {
            return _oAuthToken;
        }
    }

    [Serializable]
    public class OnLoginWithDiscordEvent : UnityEvent { }

    [Serializable]
    public class OnRateLimitEvent : UnityEvent<int, int, ApiEndpoint> { }
}