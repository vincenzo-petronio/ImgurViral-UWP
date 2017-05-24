using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP
{
    static class Constants
    {
        // API TOKEN
        public const String API_CLIENTID = "";
        public const String API_SECRET = "";
        public const String AUTH_LOCALSETTINGS_FILENAME = "AUTH_LOCAL";

        // AUTH
        public const String AUTH_ACCESS_TOKEN = "access_token";
        public const String AUTH_EXPIRES = "expires_in";
        public const String AUTH_TYPE = "token_type";
        public const String AUTH_REFRESH = "refresh_token";
        public const String AUTH_ACCOUNT_USERNAME = "account_username";
        public const String AUTH_ACCOUNT_ID = "account_id";
        public const String AUTH_CLIENT_ID = "client_id";
        public const String AUTH_CLIENT_SECRET = "client_secret";
        public const String AUTH_GRANT_TYPE = "grant_type";

        // ENDPOINT
        private const String ENDPOINT_API_BASE = "https://api.imgur.com/3/";
        public const String ENDPOINT_IMGUR_BASE = "http://imgur.com/";
        public const String ENDPOINT_API_AUTHORIZE = "https://api.imgur.com/oauth2/authorize?client_id={0}&response_type=token";
        public const String ENDPOINT_API_REFRESH_BASE = "https://api.imgur.com/oauth2/token";
        public const String ENDPOINT_API_GALLERY_VIRAL = ENDPOINT_API_BASE + "gallery/hot/viral/0.json";
        public const String ENDPOINT_API_ALBUMIMAGECOMMENTS = ENDPOINT_API_BASE + "gallery/image/{0}/comments/{1}";
    }
}
