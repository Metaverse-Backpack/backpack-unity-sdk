using UnityEngine;

namespace Bkpk
{
    public class BkpkUpdateFetch : MonoBehaviour
    {
        public BkpkAvatar Avatar;

        public void FixedUpdate()
        {
            Avatar.Tick();
        }
    }
}
