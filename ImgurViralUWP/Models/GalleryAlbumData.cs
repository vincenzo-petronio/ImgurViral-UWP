using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Model for Gallery Album API Endpoint http://api.imgur.com/3/gallery/album/{id}
    /// </summary>
    class GalleryAlbumData
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 DateTime { get; set; }
        public String Cover { get; set; }
        [JsonProperty(PropertyName = "cover_width")]
        public Int32 CoverWidth { get; set; }
        [JsonProperty(PropertyName = "cover_height")]
        public Int32 CoverHeight { get; set; }
        [JsonProperty(PropertyName = "account_url")]
        public String AccountUrl { get; set; }
        [JsonProperty(PropertyName = "account_id")]
        public Int32 AccountId { get; set; }
        public String Privacy { get; set; }
        public String Layout { get; set; }
        public Int32 Views { get; set; }
        public String Link { get; set; }
        public Int32 Ups { get; set; }
        public Int32 Downs { get; set; }
        public Int32 Score { get; set; }
        [JsonProperty(PropertyName = "is_album")]
        public Boolean IsAlbum { get; set; }
        private String Vote { get; set; }
        [JsonProperty(PropertyName = "favorite")]
        public Boolean IsFavorite { get; set; }
        [JsonProperty(PropertyName = "nsfw")]
        public Boolean IsNsfw { get; set; }
        [JsonProperty(PropertyName = "comment_count")]
        public Int32 CommentCount { get; set; }
        [JsonProperty(PropertyName = "images_count")]
        public Int32 ImagesCount { get; set; }
        public List<ImageData> Images { get; set; }
    }
}
