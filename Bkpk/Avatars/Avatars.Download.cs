// Downloader for Avatar Connect.
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace Bkpk
{
    public static partial class Avatars
    {
        // URL sanity check
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            if (url.StartsWith("http://") || url.StartsWith("https://"))
                return true;
            return false;
        }

        // Request downloads, returns the downloadHandler.
        public static async Task<object> Download(string url)
        {
            if (!IsValidUrl(url))
                throw new BkpkException(BkpkErrors.INVALID_URL);

            // Construct the request
            UnityWebRequest request = UnityWebRequest.Get(url);

            // Send the request
            request.SendWebRequest();

            // Wait for the request to complete
            while (!request.isDone)
            {
                await Task.Yield();
            }

            // Check for errors
            if (
                request.result != UnityWebRequest.Result.Success
                || request.result == UnityWebRequest.Result.ConnectionError
                || request.result == UnityWebRequest.Result.ProtocolError
                || request.result == UnityWebRequest.Result.DataProcessingError
            )
            {
                throw new BkpkException(BkpkErrors.DOWNLOAD_FAILED);
            }

            return request.downloadHandler.data;
        }
    }
}
