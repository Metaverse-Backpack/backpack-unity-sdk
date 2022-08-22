using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public class ProviderRegistry
    {
        // Global module registry.
        public AvatarLoaderModule[] GetModules()
        {
            return new AvatarLoaderModule[]
            {
                new ReadyPlayerMeModule(),
                new MeebitsModule(),
                new CryptoAvatarsModule()
            };
        }
    }
}
