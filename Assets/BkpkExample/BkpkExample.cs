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
            Bkpk.AvatarInfo info = new Bkpk.AvatarInfo
            {
                id = "XXX",
                uri = "https://cdn.mona.gallery/e6db7ec3-d1c8-4935-86ce-b339a5d11eb6.vrm",
                format = "vrm",
                type = "humanoid",
                provider = "meebits",
                metadata = "",
            };

            Bkpk.BkpkAvatar avatar = await Bkpk.Avatars.LoadAvatar(info);
        }
    }
}
