using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Model for Gallery Image API Endpoint https://api.imgur.com/3/gallery/image/{id}
    /// </summary>
    public class GalleryImageData
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 DateTime { get; set; }
        public String Type { get; set; }
        [JsonProperty(PropertyName = "animated")]
        public Boolean IsAnimated { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
        public Int32 Size { get; set; }
        public Int32 Views { get; set; }
        public Int64 Bandwidth { get; set; }
        public String DeleteHash { get; set; }
        public String Link { get; set; }
        public String Gifv { get; set; }
        public String Mp4 { get; set; }
        public String Webm { get; set; }
        public String Looping { get; set; }
        private String Vote { get; set; }
        [JsonProperty(PropertyName = "favorite")]
        public Boolean IsFavorite { get; set; }
        [JsonProperty(PropertyName = "nsfw")]
        public Boolean IsNsfw { get; set; }
        [JsonProperty(PropertyName = "comment_count")]
        public Int32 CommentCount { get; set; }
        public String Section { get; set; }
        [JsonProperty(PropertyName = "account_url")]
        public String AccountUrl { get; set; }
        [JsonProperty(PropertyName = "account_id")]
        public Int32? AccountId { get; set; }
        public Int32 Ups { get; set; }
        public Int32 Downs { get; set; }
        public Int32 Score { get; set; }
        [JsonProperty(PropertyName = "is_album")]
        public Boolean IsAlbum { get; set; }
    }
}
