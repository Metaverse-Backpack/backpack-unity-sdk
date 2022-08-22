using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public static partial class Avatars
    {
        public static async Task<PaginationResponse<AvatarInfo>> GetAvatars(int page = 1)
        {
            PaginationResponse<AvatarInfo> response = await Client.Get<
                PaginationResponse<AvatarInfo>
            >("/avatars?page=" + page);
            return response;
        }

        public static async Task<AvatarInfo> GetDefaultAvatar()
        {
            PaginationResponse<AvatarInfo> response = await GetAvatars();
            if (response.results.Length == 0)
                throw new BkpkException(BkpkErrors.NO_AVATARS);
            return response.results[0];
        }
    }

    [System.Serializable]
    public class AvatarInfo
    {
        public string id;
        public string uri;
        public string format;
        public string type;
        public string provider;
        public string metadata;
    }

    [System.Serializable]
    public class PaginationResponse<T>
    {
        public T[] results;
        public int page;
        public int pageCount;
        public int total;
    }
}
