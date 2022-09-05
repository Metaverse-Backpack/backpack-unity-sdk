using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Reflection;

namespace Bkpk
{
    public  class Auth : MonoBehaviour
    {
         System.Action<AuthorizationCodeResponse> _onAuthorizationCode = null;
         string _accessToken = null;
         string _state = null;
         string _code = null;
         bool _authorized = false;

        private static Auth instance;

        public static Auth Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Auth();
                }
                return instance;
            }
        }


        private Auth() {
            string state = "";
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int i = 0; i < 16; i++)
            {
                state += characters[Random.Range(0, characters.Length)];
            }
            _state = state;
        }


        public string AccessToken
        {
            get
            {
                if (_accessToken == null)
                    throw new BkpkException(BkpkErrors.NOT_AUTHENTICATED);

                return _accessToken;
            }
            set { _accessToken = value; }
        }

#if UNITY_WEBGL
        public  async void RequestAuthorization(System.Action<AuthorizationCodeResponse> onAuthorizationCode)
        {
            _onAuthorizationCode = onAuthorizationCode;
            BkpkWebInterface.InitializeSDK(Config.ClientId, Config.ResponseType, Config.BkpkUrl, Config.WebSdkUrl, _state);
        }

        public  async void OnAuthorizationCode(string code)
        {
            AuthorizationCodeResponse authorizationCodeResponse = new AuthorizationCodeResponse
            {
                code = response.code;
                state = _state;
            };
            _onAuthorizationCode(authorizationCodeResponse);
        }
#endif

        public async Task<string> GetActivationCode(System.Action<AuthorizationCodeResponse> onAuthorizationCode)
        {
            _onAuthorizationCode = onAuthorizationCode;
            ActivationCodeRequest body = new ActivationCodeRequest
            {
                clientId = Config.ClientID,
                scopes = new string[] { "backpacks:read" },
                state = _state,
            };

            ActivationCodeResponse response = await Client.Post<ActivationCodeResponse>("/oauth/activation", body);
            
            _code = response.activationCode;
            
            // Start checking to see if authorization code has been linked and authorized
            StartCoroutine(CheckAuthorizationCodeLoop());

            return _code;
        }

        IEnumerator CheckAuthorizationCodeLoop()
        {
            _authorized = false;
            while (_authorized == false)
            {
                Task task = CheckAuthorizationCode();
                yield return new WaitUntil(() => task.IsCompleted);
                yield return new WaitForSeconds(3);
            }
        }

        async Task CheckAuthorizationCode()
        {
            // TODO: Implement support for token flow here.
            // TODO: Should also need to pass up client ID in request here.
            ActivationCodeResponse response = await Client.Get<ActivationCodeResponse>("/oauth/activation/" + _code);

            var t = response.GetType();
            PropertyInfo p = t.GetProperty("activationCode");
            if (p == null) return;
            
            _authorized = true;
            
            AuthorizationCodeResponse authorizationCodeResponse = new AuthorizationCodeResponse
            {
                code = response.authorizationCode,
                state = _state
            };
            
            _onAuthorizationCode(authorizationCodeResponse);
        }
    }

    [System.Serializable]
    public class AuthorizationCodeResponse
    {
        public string code;
        public string state;
    }

    [System.Serializable]
    class ActivationCodeRequest
    {
        public string clientId;
        public string responseType;
        public string[] scopes;
        public string state;
    }


    [System.Serializable]
    class ActivationCodeResponse
    {
        public string activationCode;
        public string? authorizationCode;
    }
}