using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Wrapper Model for Image / Album comments data
    /// </summary>
    public class AlbumImageComments
    {
        public IEnumerable<AlbumImageCommentsData> Data { get; set; }
        public Int32 Status { get; set; }
        public Boolean Success { get; set; }
    }
}
