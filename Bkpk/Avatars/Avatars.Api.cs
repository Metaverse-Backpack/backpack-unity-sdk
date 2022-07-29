using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public partial static class Avatars
    {
        public static async PaginationResponse<AvatarMetadata> GetAvatars(int page = 1)
        {
           PaginationResponse<AvatarMetadata> response = await Client.Get("/avatars?page=" + page);
           return response;
        }

        public static async AvatarMetadata GetDefaultAvatar()
        {
            PaginationResponse<AvatarMetadata> response = await GetAvatars();
            if (response.results.Length == 0)
                throw new BkpkException(BkpkErrors.NO_AVATARS);
            return response.results[0];
        }
    }

    [System.Serializable]
    public class AvatarMetadata
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
        public number page;
        public number pageCount;
        public number total;
    }
}