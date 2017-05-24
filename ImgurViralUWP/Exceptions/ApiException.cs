using ImgurViralUWP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Exceptions
{
    /// <summary>
    /// Custom Exception per gestire errori delle API di Imgur
    /// <see cref="http://api.imgur.com/errorhandling"/>
    /// </summary>
    class ApiException : Exception
    {
        public string Msg { get; set; }

        public ApiException(string msg)
            : base(msg)
        {
            this.Msg = msg;
        }
    }
}
