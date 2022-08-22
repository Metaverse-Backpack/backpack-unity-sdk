namespace Bkpk
{
    [System.Serializable]
    public class BkpkException : System.Exception
    {
        public BkpkException() : base() { }

        public BkpkException(string message) : base(message) { }

        public BkpkException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        public BkpkException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
        ) : base(info, context) { }
    }

    public static class BkpkErrors
    {
        public static string NO_MODULE_FOUND = "No module suitable for loading the provided avatar";

        // Internal
        public static string INVALID_URL = "Invalid Avatar URL provided";
        public static string DOWNLOAD_FAILED = "Avatar file download failed";

        // Configuration Errors
        public static string NO_CLIENT_ID = "You must provide your Client ID to use this SDK";

        // Authentication Errors
        public static string NOT_AUTHENTICATED =
            "You must authenticate the user before this operation can be used";
        public static string USER_REJECTED = "The user rejected your authorization request";

        // API Errors
        public static string NO_AVATARS = "No available avatars for this user";
    }
}
