using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public static partial class Avatars
    {
        private ProviderRegistry Providers;

        Avatars()
        {
            Providers = new ProviderRegistry();
        }

        private AvatarLoaderModule[] GetFormatRegistry()
        {
            return new AvatarLoaderModule[] { new GlbLoaderModule(), new VrmLoaderModule() };
        }

        // Get partner module by name, returns null if not found.
        private AvatarLoaderModule GetModule(AvatarInfo avatarInfo)
        {
            AvatarLoaderModule[] providerModules = Providers.GetModules();
            foreach (AvatarLoaderModule module in providerModules)
            {
                if (module.ModuleName == avatarInfo.provider)
                {
                    return module;
                }
            }

            AvatarLoaderModule[] formatModules = GetFormatRegistry();
            foreach (AvatarLoaderModule module in formatModules)
            {
                if (module.ModuleName == avatarInfo.format)
                {
                    return module;
                }
            }
        }

        public LoadAvatar(AvatarInfo avatarInfo, GameObject targetGameObject = null)
        {
            AvatarLoaderModule module = GetModule(avatarInfo);

            module.RequestAvatar(avatarInfo, targetGameObject);
        }
    }
}
