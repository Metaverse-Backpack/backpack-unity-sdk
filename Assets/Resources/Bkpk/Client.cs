using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public static class Client
    {
        public static async Task<T> Get<T>(string endpoint, bool authenticated = false)
        {
            string url = Config.BkpkApiUri + endpoint;
            UnityWebRequest www = UnityWebRequest.Get(url);

            if (authenticated)
                www.SetRequestHeader("Authorization", "Bearer " + Auth.Instance.AccessToken);

            www.SendWebRequest();
            while (!www.isDone)
            {
                // waiting for response
                await Task.Yield();
            }

            return JsonUtility.FromJson<T>(www.downloadHandler.text);
        }

        public static async Task<T> Post<T>(string endpoint, object data)
        {
            string url = Config.BkpkApiUri + endpoint;
            UnityWebRequest www = UnityWebRequest.Post(url, "");
            string json = JsonUtility.ToJson(data);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            while (!www.isDone)
            {
                await Task.Yield();
            }

            return JsonUtility.FromJson<T>(www.downloadHandler.text);
        }
    }
}
