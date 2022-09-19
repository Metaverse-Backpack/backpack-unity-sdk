using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleLoader
{
    public class SimpleLoader : MonoBehaviour
    {
        bool Initialized = false;

        void Start()
        {
            Bkpk.Config.WebSdkUrl = "https://owner.com.ngrok.io/JsSdk/dist/index.js";
            Bkpk.Config.BkpkApiUrl = "http://localhost:3000";
            Bkpk.Config.BkpkUrl = "http://localhost:8080";
            Bkpk.Config.ClientID = "20a3c6c5-0dbd-407e-9e54-d0d5cd31a6cf";
            Bkpk.Config.IpfsGateway = "https://ipfs.mona.gallery";

            // Bkpk.Auth.Instance.RequestAuthorization(OnAuthorized);
            OnAuthorized("XXX");
        }

        async void OnAuthorized(string token)
        {
            // Bkpk.AvatarInfo avatar = await Bkpk.Avatars.GetDefaultAvatar();
            // await Bkpk.Avatars.LoadAvatar(avatar);

            Bkpk.AvatarInfo rpm = new Bkpk.AvatarInfo
            {
                id = "XXX",
                content = "bafybeiezpxc6nrxe6hdmlo7gmi46aigt24vhkt233qigvnlfybo7hznjl4",
                metadata = new Bkpk.AvatarMetadata
                {
                    source = "XXX",
                    fileFormat = "glb",
                    type = "humanoid"
                }
            };

            Bkpk.AvatarInfo meebits = new Bkpk.AvatarInfo
            {
                id = "XXX",
                content = "bafybeibphpz3umc7s66zxoyo5why2rdgrnvo3wzcyj64ktt5ssvzob67vm",
                metadata = new Bkpk.AvatarMetadata
                {
                    source = "XXX",
                    fileFormat = "vrm",
                    bodyType = "humanoid",
                    type = "humanoid"
                }
            };

            Bkpk.AvatarInfo ca = new Bkpk.AvatarInfo
            {
                id = "XXX",
                content = "bafybeiglu4e4grhzw5kzek4epi32jqugki5o6q7kzh4jcx2icctowuiyva",
                metadata = new Bkpk.AvatarMetadata
                {
                    source = "XXX",
                    fileFormat = "vrm",
                    bodyType = "humanoid",
                    type = "humanoid"
                }
            };

            await Bkpk.Avatars.LoadAvatar(meebits);
        }

        void Update()
        {
            if (Input.GetMouseButton(0) && !Initialized)
            {
                // Bkpk.Auth.Instance.RequestAuthorization(OnAuthorized);
                Initialized = true;
            }
        }
    }
}
