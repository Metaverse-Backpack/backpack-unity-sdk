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
            Bkpk.AvatarInfo meebits = new Bkpk.AvatarInfo
            {
                id = "XXX",
                uri = "https://cdn.mona.gallery/e6db7ec3-d1c8-4935-86ce-b339a5d11eb6.vrm",
                format = "vrm",
                type = "humanoid",
                provider = "meebits",
                metadata = "",
            };

            Bkpk.AvatarInfo readyplayerme = new Bkpk.AvatarInfo
            {
                id = "XXX",
                uri =
                    "https://d1a370nemizbjq.cloudfront.net/0dccee22-f9db-44ca-a49d-deb8ca27aae5.glb",
                format = "glb",
                type = "humanoid",
                provider = "readyplayerme",
                metadata =
                    "{\"bodyType\": \"fullbody\",\"outfitGender\": \"masculine\",\"outfitVersion\": 2}",
            };

            Bkpk.BkpkAvatar meebitsAvatar = await Bkpk.Avatars.LoadAvatar(meebits);
            Bkpk.BkpkAvatar rpmAvatar = await Bkpk.Avatars.LoadAvatar(readyplayerme);
            rpmAvatar.AvatarObject.transform.localPosition = new Vector3(2, 0, 0);
        }
    }
}
