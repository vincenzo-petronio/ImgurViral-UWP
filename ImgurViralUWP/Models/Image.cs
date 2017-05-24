using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Wrapper Model for Image data
    /// </summary>
    class Image
    {
        public IEnumerable<ImageData> Data { get; set; }
        public Int32 Status { get; set; }
        public Boolean Success { get; set; }
    }
}
