#if UNITY_WEBGL
using UnityEngine;
using System.Runtime.InteropServices;

namespace Bkpk
{
    public class WebInterface : MonoBehaviour
    {
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

        void OnAccessToken(string accessToken)
        {
            Auth.Instance.OnAccessToken(accessToken);
        }

        void OnAuthorizationCode(string authorizationCode)
        {
            Auth.Instance.OnAuthorizationCode(authorizationCode);
        }

        void OnUserRejected()
        {
            throw new BkpkException(BkpkErrors.USER_REJECTED);
        }
    }
}

#endif
