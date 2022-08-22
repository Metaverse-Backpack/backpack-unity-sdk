using UnityEngine;

namespace Bkpk
{
    // Base class for module avatar
    public class BkpkAvatar
    {
        // Avatar object
        public GameObject AvatarObject;

        // Animation
        public Animator Animator;

        // Fixed update
        public virtual void Tick() { }
    }
}
