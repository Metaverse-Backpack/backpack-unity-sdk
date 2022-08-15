using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    protected static class Client
    {
        public async Task<T> Get(string endpoint, bool authenticated = false)
        {
            string url = Config.BkpkApiUri + endpoint;
            UnityWebRequest www = UnityWebRequest.Get(url);

            if (authenticated)
                www.SetRequestHeader("Authorization", "Bearer " + Auth.AccessToken);

            www.SendWebRequest();
            while (!www.isDone)
            {
                // waiting for response
                Task.Yield();
            }

            return JsonUtility.FromJson<T>(www.downloadHandler.text);
        }

        public Task<T> Post(string endpoint, object data)
        {
            string url = Config.BkpkApiUri + endpoint;
            UnityWebRequest www = UnityWebRequest.Post(url);
            string json = JsonUtility.ToJson(data);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            while (!www.isDone)
            {
                Task.Yield();
            }

            return JsonUtility.FromJson<T>(www.downloadHandler.text);
        }
    }
}
