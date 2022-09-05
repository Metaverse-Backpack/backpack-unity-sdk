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
            Bkpk.AvatarInfo cryptoAvatars = new Bkpk.AvatarInfo
            {
                source =
                    "https://alvevault.s3.eu-central-1.amazonaws.com/CryptoAvatars_Orion_CryptoAvatars_Orion.vrm",
                fileFormat = "vrm",
                type = "humanoid",
            };

            Bkpk.AvatarInfo meebits = new Bkpk.AvatarInfo
            {
                source = "https://cdn.mona.gallery/e6db7ec3-d1c8-4935-86ce-b339a5d11eb6.vrm",
                fileFormat = "vrm",
                type = "humanoid",
            };

            Bkpk.AvatarInfo readyplayerme = new Bkpk.AvatarInfo
            {
                source =
                    "https://d1a370nemizbjq.cloudfront.net/0dccee22-f9db-44ca-a49d-deb8ca27aae5.glb",
                fileFormat = "glb",
                type = "humanoid",
            };

            Bkpk.BkpkAvatar meebitsAvatar = await Bkpk.Avatars.LoadAvatar(meebits);
            Bkpk.BkpkAvatar rpmAvatar = await Bkpk.Avatars.LoadAvatar(readyplayerme);
            Bkpk.BkpkAvatar caAvatar = await Bkpk.Avatars.LoadAvatar(cryptoAvatars);
            rpmAvatar.AvatarObject.transform.localPosition = new Vector3(2, 0, 0);
            caAvatar.AvatarObject.transform.localPosition = new Vector3(-2, 0, 0);

            var rotation = Quaternion.Inverse(meebitsAvatar.AvatarObject.transform.localRotation);

            meebitsAvatar.AvatarObject.transform.localRotation = rotation;
            rpmAvatar.AvatarObject.transform.localRotation = rotation;
            caAvatar.AvatarObject.transform.localRotation = rotation;
        }
    }
}
