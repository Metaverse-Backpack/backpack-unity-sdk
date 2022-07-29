using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    // Base class for module avatar
    public static class Avatar
    {
        // Avatar object
        public GameObject AvatarObject;

        // Animation
        public Animator Animator;
        public AvatarEyeManager EyeManager;

        // Avatar activation
        public virtual void Activate()
        {
            EyeManager = new AvatarEyeManager();
        }

        // Fixed update
        public virtual void Tick() { }
    }

    [System.Serializable]
    public class BkpkAvatar
    {
        public string id;
        public string uri;
        public string format;
        public string type;
        public string provider;
        public string metadata;
    }
}