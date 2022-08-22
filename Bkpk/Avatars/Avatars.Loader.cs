using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public static partial class Avatars
    {
        public static ProviderRegistry Providers = new ProviderRegistry();

        static AvatarLoaderModule[] GetFormatRegistry()
        {
            return new AvatarLoaderModule[] { new GlbLoaderModule(), new VrmLoaderModule() };
        }

        // Get partner module by name, returns null if not found.
        static AvatarLoaderModule GetModule(AvatarInfo avatarInfo)
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

            throw new BkpkException(BkpkErrors.NO_MODULE_FOUND);
        }

        public static async Task<BkpkAvatar> LoadAvatar(
            AvatarInfo avatarInfo,
            GameObject targetGameObject = null
        )
        {
            AvatarLoaderModule module = GetModule(avatarInfo);

            return await module.RequestAvatar(avatarInfo, targetGameObject);
        }
    }
}
