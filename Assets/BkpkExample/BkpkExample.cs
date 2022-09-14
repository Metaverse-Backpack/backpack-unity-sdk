using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BkpkExample
{
    public partial class BkpkExample : MonoBehaviour
    {
        void Start()
        {
            Bkpk.Config.WebSdkUrl = "https://owner.com.ngrok.io/JsSdk/dist/index.js";
            Bkpk.Config.BkpkApiUrl = "http://localhost:3000";
            Bkpk.Config.BkpkUrl = "http://localhost:8080";
        }

        async void OnAuthorized(string token)
        {
            Bkpk.AvatarInfo avatar = await Bkpk.Avatars.GetDefaultAvatar();

            await Bkpk.Avatars.LoadAvatar(avatar);
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Bkpk.Auth.Instance.RequestAuthorization(OnAuthorized);
            }
        }
    }
}
