using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public abstract class Auth
    {
        private Action<AuthorizationCodeResponse> _onCodeAuthorized = null;
        private static string _accessToken = null;
        private static Random _random = new Random();
        private static string _state = null;
        private static string _code = null;
        private static bool _authorized = false;

        public static string AccessToken
        {
            get {
                if (_accessToken == null)
                    throw new BkpkException(BkpkErrors.NOT_AUTHENTICATED);
                
                return _accessToken;
            }
            set { _accessToken = value; }
        }

#if UNITY_WEBGL
        public static async void RequestAuthorization(Action<AuthorizationCodeResponse> onAuthorizationCode)
        {
            _onAuthorizationCode = onAuthorizationCode;
            _state = CreateState();
            BkpkWebInterface.InitializeSDK(Config.ClientId, Config.ResponseType, Config.BkpkUrl, Config.WebSdkUrl, _state);
        }

        protected static async void OnAuthorizationCode(string code)
        {
            AuthorizationCodeResponse authorizationCodeResponse = new AuthorizationCodeResponse
            {
                code = response.code;
                state = _state;
            };
            _onAuthorizationCode(authorizationCodeResponse);
        }
#endif

        public static async Task<string> GetActivationCode(Action<AuthorizationCodeResponse> onAuthorizationCode)
        {
            _onAuthorizationCode = onAuthorizationCode;
            _state = CreateState();
            ActivationCodeRequest body = new ActivationCodeRequest
            {
                clientId = Config.ClientID;
                responseType = Config.ResponseType;
                scopes = ["avatars:read", "backpacks:read"];
                state = _state;
            };

            ActivationCodeResponse response = await Client.Post<ActivationCodeResponse>("/oauth/activation-code", body);
            
            // Start checking to see if authorization code has been linked and authorized
            StartCoroutine(CheckAuthorizationCodeLoop());

            _code = response.code;
            
            return _code;
        }

        private static IEnumerator CheckAuthorizationCodeLoop()
        {
            _authorized = false;
            while (_authorized == false)
            {
                Task task = CheckAuthorizationCode();
                yield return new WaitUntil(() => task.IsCompleted);
                yield return new WaitForSeconds(3);
            }
        }

        private static async Task CheckAuthorizationCode()
        {
            AuthorizationResponse response = await Client.Get<AuthorizationResponse>("/oauth/authorization?clientId=" + Config.ClientID + "&state=" + _state + "&code=" + _code);
            if (Config.ResponseType == "token" && response.token != null)
            {
                _authorized = true;
                AccessToken = response.token;
            } 
            else if (Config.ResponseType == "code" && response.code != null)
            {
                _authorized = true;
                AuthorizationCodeResponse authorizationCodeResponse = new AuthorizationCodeResponse
                {
                    code = response.code;
                    state = _state;
                };
                _onAuthorizationCode(authorizationCodeResponse);
            }
        }

        private static string CreateState()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public class AuthorizationCodeResponse
    {
        public string code;
        public string state;
    }

    [System.Serializable]
    private class ActivationCodeRequest
    {
        public string clientId;
        public ResponseTypeOption responseType;
        public string[] scopes;
        public string state;
    }


    [System.Serializable]
    private class ActivationCodeResponse
    {
        public string code;
    }

    [System.Serializable]
    private class AuthorizationResponse
    {
        public string token = null;
        public string code = null;
    }
}