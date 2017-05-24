using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Wrapper Model for Gallery Image data
    /// </summary>
    public class GalleryImage
    {
        public IEnumerable<GalleryImageData> Data { get; set; }
        public Int32 Status { get; set; }
        public Boolean Success { get; set; }
    }
}
