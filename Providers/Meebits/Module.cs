using UnityEngine;

namespace Bkpk
{
    protected class MeebitsModule : VrmLoaderModule
    {
        public MeebitsModule()
        {
            ModuleName = "meebits";
            TextureFilterMode = FilterMode.Point;
        }
    }
}
