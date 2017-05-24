using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Exceptions
{
    /// <summary>
    /// Custom Exception per gestire errori legati alla connettività
    /// </summary>
    class NetworkException : Exception
    {
        public string Msg { get; set; }

        public NetworkException(string msg)
            : base(msg)
        {
            this.Msg = msg;
        }
    }
}
