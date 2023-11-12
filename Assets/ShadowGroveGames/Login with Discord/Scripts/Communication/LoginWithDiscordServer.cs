using ShadowGroveGames.LoginWithDiscord.Scripts.Extensions;
using ShadowGroveGames.LoginWithDiscord.Scripts.Struct;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace ShadowGroveGames.LoginWithDiscord.Scripts.Communication
{
    public class LoginWithDiscordServer : IDisposable
    {
        private Listener _listener;

        /// <summary>
        /// A list of possible ports which can be used for discord callbacks
        /// </summary>
        private HashSet<int> _callbackPorts;

        /// <summary>
        /// This port is used for the callback from discord
        /// </summary>
        public int ActivePort { get; private set; } = 0;

        /// <summary>
        /// Server state key to validate the requests come from discord
        /// </summary>
        private string _serverStateKey;

        /// <summary>
        /// Discord application id
        /// </summary>
        private string _applicationId;

        /// <summary>
        /// Log Level for the callback server
        /// </summary>
        private ListenerLogLevel _logLevel = ListenerLogLevel.Error;

        /// <summary>
        /// The processRequestQueue contains open requests that have not yet been processed.
        /// </summary>
        private Queue<HttpListenerContext> _processRequestQueue = new Queue<HttpListenerContext>();

        /// <summary>
        /// Events
        /// </summary>
        public event OnLoginWithDiscordSuccess OnSuccess;
        public event OnLoginWithDiscordError OnFailure;

        public LoginWithDiscordServer(HashSet<int> callbackPorts, ListenerLogLevel logLevel, string serverStateKey, string applicationId)
        {
            _callbackPorts = callbackPorts;
            _logLevel = logLevel;
            _serverStateKey = serverStateKey;
            _applicationId = applicationId;
        }

        // Start is called before the first frame update
        public bool Start()
        {
            _listener = new Listener(_logLevel);

            foreach (int callbackPort in _callbackPorts)
            {
                if (!PortInUse(callbackPort))
                {
                    _listener.AddListeningAddress(callbackPort);
                    ActivePort = callbackPort;
                    break;
                }
            }

            if (ActivePort == 0)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogError("All specified callback ports are used! Please select less popular ports.");

                return false;
            }


            _listener.OnReceiveRequestEvent += OnReceiveRequestEvent;
            return _listener.Start();
        }

        // Update is called once per frame
        public void Stop()
        {
            _processRequestQueue.Clear();
            _listener.OnReceiveRequestEvent -= OnReceiveRequestEvent;
            _listener.Stop();
        }

        /// <summary>
        /// In order to process the requests that have been accepted in the Async mode, we need to process them here one by one
        /// </summary>
        public void Process()
        {
            if (_processRequestQueue.Count == 0)
                return;

            var context = _processRequestQueue.Dequeue();

            try
            {
                if (!IPAddress.IsLoopback(context.Request.RemoteEndPoint.Address))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Close();
                    return;
                }

                ProcessReceivedRequest(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.TextResponse($"{ex.Message}\n\nStacktrace:\n{ex.StackTrace}");
            }

            context.Response.Close();
        }

        private bool OnReceiveRequestEvent(HttpListenerContext context)
        {
            _processRequestQueue.Enqueue(context);

            return false;
        }

        private void ProcessReceivedRequest(HttpListenerContext context)
        {
            var absolutePath = context.Request.Url.AbsolutePath.TrimEnd('/');

            if (context.Request.HttpMethod == HttpConstants.MethodGet && absolutePath == "/error")
            {
                HandleShowErrorPage(context);
                return;
            }

            if (context.Request.HttpMethod == HttpConstants.MethodGet && absolutePath == "/background.png")
            {
                HandleImage(context, "background");
                return;
            }

            if (context.Request.HttpMethod == HttpConstants.MethodGet && absolutePath == "/circle-mark.png")
            {
                HandleImage(context, "circle-mark");
                return;
            }

            if (context.Request.HttpMethod == HttpConstants.MethodPost && absolutePath == "/submit")
            {
                HandleReceiveToken(context);
                return;
            }


            HandleShowDefaultPage(context);
        }

        private void HandleShowErrorPage(HttpListenerContext context)
        {
            var content = (TextAsset)Resources.Load("wwwroot/login-with-discord-success-page", typeof(TextAsset));
            context.Response.HtmlResponse(content.text);
        }

        private void HandleImage(HttpListenerContext context, string imageKey)
        {
            var loadedTexture = (Texture2D)Resources.Load("login-with-discord-wwwroot/" + imageKey, typeof(Texture2D));
            context.Response.ImageResponse(loadedTexture);
        }

        private void HandleReceiveToken(HttpListenerContext context)
        {
            if (context.Request.ContentType != HttpConstants.ContentTypeApplicationJson || context.Request.ContentLength64 == 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                OnFailure?.Invoke();
                return;
            }

            try
            {
                var body = "";

                using (Stream receiveStream = context.Request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, context.Request.ContentEncoding))
                    {
                        body = readStream.ReadToEnd();
                    }
                }

                OAuthToken oAuthToken = JsonUtility.FromJson<OAuthToken>(body);
                oAuthToken.UtcCreatedAt = DateTime.UtcNow;
                oAuthToken.UtcExpiresAt = DateTime.UtcNow.AddSeconds(oAuthToken.ExpiresIn);
                oAuthToken.ApplicationId = _applicationId;

                if (oAuthToken.State != _serverStateKey)
                {
                    if (_logLevel >= ListenerLogLevel.Warning)
                        Debug.LogWarning("Recive invalid state!");

                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    OnFailure?.Invoke();
                    return;
                }

                if (_logLevel >= ListenerLogLevel.Info)
                    Debug.Log($"Receive \"{oAuthToken.TokenType}\" oauth token from discord.");

                OnSuccess?.Invoke(oAuthToken);
            }
            catch (Exception ex)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogException(ex);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                OnFailure?.Invoke();
            }
        }

        private void HandleShowDefaultPage(HttpListenerContext context)
        {
            var content = (TextAsset)Resources.Load("login-with-discord-wwwroot/default-page", typeof(TextAsset));
            context.Response.HtmlResponse(content.text);
        }

        public void Dispose()
        {
            Stop();
        }

        private bool PortInUse(int port)
        {
            try
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

                client.Connect("127.0.0.1", port, TimeSpan.FromMilliseconds(250));
                if (client.Connected)
                    return true;

            }
            catch
            { }

            return false;
        }
    }

    public delegate void OnLoginWithDiscordSuccess(OAuthToken oAuthToken);

    public delegate void OnLoginWithDiscordError();
}
