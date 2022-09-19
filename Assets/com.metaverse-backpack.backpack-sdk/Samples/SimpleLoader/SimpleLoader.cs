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
            Bkpk.Auth.Instance.RequestAuthorization(OnAuthorized);
        }

        async void OnAuthorized(string token)
        {
            Bkpk.AvatarInfo avatar = await Bkpk.Avatars.GetDefaultAvatar();
            await Bkpk.Avatars.LoadAvatar(avatar);
        }

        void Update()
        {
            if (Input.GetMouseButton(0) && !Initialized)
            {
                Bkpk.Auth.Instance.RequestAuthorization(OnAuthorized);
                Initialized = true;
            }
        }
    }
}
