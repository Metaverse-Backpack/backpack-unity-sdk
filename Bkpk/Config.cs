using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public enum ResponseTypeOption
    {
        token,
        code
    }

    public static class Config
    {
        private static string _bkpkApiUri = "https://api.bkpk.io";
        private static string _clientId = null;
        private static ResponseTypeOption _responseType = "token";
        private static string _bkpkUrl = "https://bkpk.io";
        private static string _WebSdkUrl = "https://cdn.jsdelivr.net/npm/@bkpk/sdk/dist/index.js";

        public string BkpkApiUri
        {
            get { return _bkpkApiUri; }
            set { _bkpkApiUri = value; }
        }

        public string BkpkUrl
        {
            get { return _bkpkUrl; }
            set { _bkpkUrl = value; }
        }

        public string WebSdkUrl
        {
            get { return _WebSdkUrl; }
            set { _WebSdkUrl = value; }
        }

        public string ClientID
        {
            get
            {
                if (_clientId == null)
                    throw new BkpkException(BkpkErrors.NO_CLIENT_ID);

                return _clientId;
            }
            set { _clientId = value; }
        }

        public ResponseTypeOption ResponseType
        {
            get { return _responseType; }
            set { _responseType = value; }
        }
    }
}