#if UNITY_WEBGL
using UnityEngine;
using System.Runtime.InteropServices;

namespace Bkpk
{
    public class WebInterface : MonoBehaviour
    {
        private bool _popupOpen = false;

        [DllImport("__Internal")]
        public static extern void InitializeSDK(
            string clientId,
            string responseType,
            string url,
            string apiUrl,
            string webSdkUrl,
            string state = null
        );

        private static WebInterface _instance;

        public static WebInterface Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("Bkpk.WebInterface");
                    _instance = go.AddComponent<WebInterface>();
                }
                return _instance;
            }
        }

        public void StartAuthentication(string responseType, string state)
        {
            InitializeSDK(
                Config.ClientID,
                responseType,
                Config.BkpkUrl,
                Config.BkpkApiUrl,
                Config.WebSdkUrl,
                state
            );
            _popupOpen = true;
        }

        void OnAccessToken(string accessToken)
        {
            Auth.Instance.OnAccessToken(accessToken);
            _popupOpen = false;
        }

        void OnAuthorizationCode(string authorizationCode)
        {
            Auth.Instance.OnAuthorizationCode(authorizationCode);
            _popupOpen = false;
        }

        void OnUserRejected()
        {
            _popupOpen = false;
            throw new BkpkException(BkpkErrors.USER_REJECTED);
        }
    }
}

#endif
