using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public static partial class Avatars
    {
        public static async Task<AvatarInfo[]> GetAvatars()
        {
            // TODO: This needs to be updated to use a graph endpoint or a paginated endpoint
            BackpackResponse response = await Client.Get<BackpackResponse>("/backpack/owner", true);
            return response.backpackItems;
        }

        public static async Task<AvatarInfo> GetDefaultAvatar()
        {
            AvatarInfo[] avatars = await GetAvatars();
            if (avatars.Length == 0)
                throw new BkpkException(BkpkErrors.NO_AVATARS);
            return avatars[0];
        }
    }

    [System.Serializable]
    public class BoneStructure
    {
        public string head;
    }

    [System.Serializable]
    public class AvatarMetadata
    {
        public string source;
        public string type;
        public string fileFormat;
        public string reference = null;
        public string bodyType = null;
        public BoneStructure boneStructure = null;
    }

    [System.Serializable]
    public class AvatarInfo
    {
        public string id;
        public string content;
        public AvatarMetadata metadata;
    }

    [System.Serializable]
    public class BackpackResponse
    {
        public AvatarInfo[] backpackItems;
    }
}
