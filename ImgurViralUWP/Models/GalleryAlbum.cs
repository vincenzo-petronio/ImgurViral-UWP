using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Wrapper Model for Gallery Album data
    /// </summary>
    class GalleryAlbum
    {
        public IEnumerable<GalleryAlbumData> Data { get; set; }
        public Int32 Status { get; set; }
        public Boolean Success { get; set; }
    }
}
