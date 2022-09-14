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
            Test();
        }

        async void Test()
        {
            Bkpk.Config.WebSdkUrl = "https://owner.com.ngrok.io/JsSdk/dist/index.js";
            Bkpk.Config.BkpkApiUrl = "http://localhost:3000";
            Bkpk.Config.BkpkUrl = "http://localhost:8080";
            Bkpk.Auth.Instance.AccessToken =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIweGVkOThiMjZhNTgwZmFjY2VhYmU2YjVkMGJjZjZiYTM5OTJjMzRhZmYiLCJiYWNrcGFjayI6Ijc4YmQ2YmFjLThiODEtNDc1Zi1iMWMxLWQ0MDhhOTViZjAwYiIsInNjb3BlcyI6ImF2YXRhcnM6cmVhZCIsImlhdCI6MTY2MzEzMjU5OSwiZXhwIjoxNjYzMTMzNDk5fQ.X-RcJtTZiWPrb0xZb10onR7udiyO7nV-xxKpgSpF_qY";

            Bkpk.AvatarInfo avatar = await Bkpk.Avatars.GetDefaultAvatar();

            await Bkpk.Avatars.LoadAvatar(avatar);
        }
    }
}
