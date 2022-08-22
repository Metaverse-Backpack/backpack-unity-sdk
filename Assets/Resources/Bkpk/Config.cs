using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Bkpk
{
    public static class ResponseTypes
    {
        public const string Token = "token";
        public const string Code = "code";
    }

    public static class Config
    {
        static string _bkpkApiUri = "https://api.bkpk.io";
        static string _clientId = null;
        static string _responseType = ResponseTypes.Token;
        static string _bkpkUrl = "https://bkpk.io";
        static string _WebSdkUrl = "https://cdn.jsdelivr.net/npm/@bkpk/sdk/dist/index.js";

        public static string BkpkApiUri
        {
            get { return _bkpkApiUri; }
            set { _bkpkApiUri = value; }
        }

        public static string BkpkUrl
        {
            get { return _bkpkUrl; }
            set { _bkpkUrl = value; }
        }

        public static string WebSdkUrl
        {
            get { return _WebSdkUrl; }
            set { _WebSdkUrl = value; }
        }

        public static string ClientID
        {
            get
            {
                if (_clientId == null)
                    throw new BkpkException(BkpkErrors.NO_CLIENT_ID);

                return _clientId;
            }
            set { _clientId = value; }
        }

        public static string ResponseType
        {
            get { return _responseType; }
            set { _responseType = value; }
        }
    }
}
