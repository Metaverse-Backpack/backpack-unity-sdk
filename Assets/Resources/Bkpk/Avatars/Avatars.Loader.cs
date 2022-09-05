using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public static partial class Avatars
    {
        static AvatarLoaderModule[] GetFormatRegistry()
        {
            return new AvatarLoaderModule[] { new GlbLoaderModule(), new VrmLoaderModule() };
        }

        // Get partner module by name, returns null if not found.
        static AvatarLoaderModule GetModule(AvatarInfo avatarInfo)
        {
            AvatarLoaderModule[] formatModules = GetFormatRegistry();
            foreach (AvatarLoaderModule module in formatModules)
            {
                if (module.ModuleName == avatarInfo.fileFormat)
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
