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
        static string _bkpkApiUrl = "https://backend.metabag.dev";
        static string _clientId = null;
        static string _bkpkUrl = "https://frontend.metabag.dev";
        static string _webSdkUrl =
            "https://cdn.jsdelivr.net/npm/@metaverse-backpack/backpack-js-sdk@0.0.2/dist/index.min.js";
        static string _ipfsGateway = "https://gateway.ipfs.io";

        public static string BkpkApiUrl
        {
            get { return _bkpkApiUrl; }
            set { _bkpkApiUrl = value; }
        }

        public static string IpfsGateway
        {
            get { return _ipfsGateway; }
            set { _ipfsGateway = value; }
        }

        public static string BkpkUrl
        {
            get { return _bkpkUrl; }
            set { _bkpkUrl = value; }
        }

        public static string WebSdkUrl
        {
            get { return _webSdkUrl; }
            set { _webSdkUrl = value; }
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
    }
}
