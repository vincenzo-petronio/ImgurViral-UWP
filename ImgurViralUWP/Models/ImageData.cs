using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Model for Gallery Album API Endpoint https://api.imgur.com/3/image/{id}
    /// </summary>
    class ImageData
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
        public Int32 Bandwidth { get; set; }
        public String DeleteHash { get; set; }
        public String Name { get; set; }
        public String Section { get; set; }
        public String Link { get; set; }
        public String Gifv { get; set; }
        public String Mp4 { get; set; }
        public String Webm { get; set; }
        public String Looping { get; set; }
        [JsonProperty(PropertyName = "favorite")]
        public Boolean IsFavorite { get; set; }
        [JsonProperty(PropertyName = "nsfw")]
        public Boolean IsNsfw { get; set; }
        public String Vote { get; set; }
        public String AccountUrl { get; set; }
        [JsonProperty(PropertyName = "account_id")]
        public Int32? AccountId { get; set; }
    }
}
