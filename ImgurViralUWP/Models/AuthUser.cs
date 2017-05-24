using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Model for Authenticated User.
    /// </summary>
    public class AuthUser
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
        
        [JsonProperty(PropertyName = "account_username")]
        public string Username { get; set; }
        
        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresToken { get; set; }
        
        [JsonProperty(PropertyName = "token_type")]
        public string TypeToken { get; set; }
        
        [JsonProperty(PropertyName = "account_id")]
        public string AccountId { get; set; }
    }
}
