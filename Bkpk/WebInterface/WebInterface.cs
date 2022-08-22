#if UNITY_WEBGL
using UnityEngine;
using System.Runtime.InteropServices;

namespace Bkpk
{
    public class BkpkWebInterface : MonoBehaviour
    {
        [DllImport("__Internal")]
        public static extern void InitializeSDK(
            string clientId,
            string responseType,
            string url,
            string apiUrl,
            string scriptUrl,
            string state
        );

        OnAccessToken(string accessToken)
        {
            Auth.Instance.AccessToken = accessToken;
        }

        OnAuthorizationCode(string authorizationCode)
        {
            Auth.OnAuthorizationCode(authorizationCode);
        }

        OnUserRejected()
        {
            throw new BkpkException(BkpkErrors.USER_REJECTED);
        }
    }
}

#endif
