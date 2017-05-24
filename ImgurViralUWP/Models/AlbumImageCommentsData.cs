using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Model for Image / Gallery Comments API Endpoint https://api.imgur.com/3/gallery/album/{id}/comments/{sort}
    /// </summary>
    public class AlbumImageCommentsData
    {
        public Int32 Id { get; set; }
        [JsonProperty(PropertyName = "image_id")]
        public String ImageId { get; set; }
        public String Comment { get; set; }
        public String Author { get; set; }
        [JsonProperty(PropertyName = "author_id")]
        public Int32 AuthorId { get; set; }
        [JsonProperty(PropertyName = "on_album")]
        public Boolean OnAlbum { get; set; }
        [JsonProperty(PropertyName = "album_cover")]
        public String AlbumCover { get; set; }
        public Int32 Ups { get; set; }
        public Int32 Downs { get; set; }
        public float Points { get; set; }
        public Int32 DateTime { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public Int32 ParentId { get; set; }
        public Boolean Deleted { get; set; }
        public String Vote { get; set; }
        public List<AlbumImageCommentsData> Children { get; set; } 

    }
}
